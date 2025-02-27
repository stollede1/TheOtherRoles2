﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AmongUs.GameOptions;
using Hazel;
using TheOtherRoles.CustomGameModes;
using TheOtherRoles.Utilities;
using UnityEngine;
using static TheOtherRoles.TheOtherRoles;

namespace TheOtherRoles.Patches;

[HarmonyPatch(typeof(RoleOptionsCollectionV08), nameof(RoleOptionsCollectionV08.GetNumPerGame))]
class RoleOptionsDataGetNumPerGamePatch
{
    public static void Postfix(ref int __result)
    {
        // Deactivate Vanilla Roles if the mod roles are active
        if (GameOptionsManager.Instance.CurrentGameOptions.GameMode == GameModes.Normal) __result = 0;
    }
}

[HarmonyPatch(typeof(IGameOptionsExtensions), nameof(IGameOptionsExtensions.GetAdjustedNumImpostors))]
class GameOptionsDataGetAdjustedNumImpostorsPatch
{
    public static void Postfix(ref int __result)
    {
        if (TORMapOptions.gameMode == CustomGamemodes.HideNSeek || TORMapOptions.gameMode == CustomGamemodes.PropHunt)
        {
            int impCount = TORMapOptions.gameMode == CustomGamemodes.HideNSeek ? Mathf.RoundToInt(CustomOptionHolder.hideNSeekHunterCount.getFloat()) : CustomOptionHolder.propHuntNumberOfHunters.getQuantity();
            __result = impCount; ; // Set Imp Num
        }
        else if (GameOptionsManager.Instance.CurrentGameOptions.GameMode == GameModes.Normal)
        {  // Ignore Vanilla impostor limits in TOR Games.
            __result = Mathf.Clamp(GameOptionsManager.Instance.CurrentGameOptions.NumImpostors, 1, 3);
        }
    }
}

[HarmonyPatch(typeof(GameOptionsData), nameof(GameOptionsData.Validate))]
class GameOptionsDataValidatePatch
{
    public static void Postfix(GameOptionsData __instance)
    {
        if (TORMapOptions.gameMode == CustomGamemodes.HideNSeek || GameOptionsManager.Instance.CurrentGameOptions.GameMode != GameModes.Normal) return;
        if (TORMapOptions.gameMode == CustomGamemodes.PropHunt)
            __instance.NumImpostors = CustomOptionHolder.propHuntNumberOfHunters.getQuantity();
        __instance.NumImpostors = GameOptionsManager.Instance.CurrentGameOptions.NumImpostors;
    }
}

[HarmonyPatch(typeof(RoleManager), nameof(RoleManager.SelectRoles))]
class RoleManagerSelectRolesPatch
{
    private static int crewValues;
    private static int impValues;
    //private static bool isEvilGuesser;
    private static List<Tuple<byte, byte>> playerRoleMap = new();
    public static bool isGuesserGamemode => TORMapOptions.gameMode == CustomGamemodes.Guesser;
    public static void Postfix()
    {
        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ResetVaribles, SendOption.Reliable, -1);
        AmongUsClient.Instance.FinishRpcImmediately(writer);
        RPCProcedure.resetVariables();
        if (TORMapOptions.gameMode == CustomGamemodes.HideNSeek || TORMapOptions.gameMode == CustomGamemodes.PropHunt || GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek) return; // Don't assign Roles in Hide N Seek
        assignRoles();
    }

    private static void assignRoles()
    {
        var data = getRoleAssignmentData();
        assignSpecialRoles(data); // Assign special roles like mafia and lovers first as they assign a role to multiple players and the chances are independent of the ticket system
        selectFactionForFactionIndependentRoles(data);
        assignEnsuredRoles(data); // Assign roles that should always be in the game next
        assignDependentRoles(data); // Assign roles that may have a dependent role
        assignChanceRoles(data); // Assign roles that may or may not be in the game last
        assignRoleTargets(data); // Assign targets for Lawyer & Prosecutor
        if (isGuesserGamemode) assignGuesserGamemode();
        assignModifiers(); // Assign modifier
        //setRolesAgain(); //brb
        if (Jackal.jackal != null) Jackal.setSwoop();
    }

    public static RoleAssignmentData getRoleAssignmentData()
    {
        // Get the players that we want to assign the roles to. Crewmate and Neutral roles are assigned to natural crewmates. Impostor roles to impostors.
        List<PlayerControl> crewmates = PlayerControl.AllPlayerControls.ToArray().ToList().OrderBy(x => Guid.NewGuid()).ToList();
        crewmates.RemoveAll(x => x.Data.Role.IsImpostor);
        List<PlayerControl> impostors = PlayerControl.AllPlayerControls.ToArray().ToList().OrderBy(x => Guid.NewGuid()).ToList();
        impostors.RemoveAll(x => !x.Data.Role.IsImpostor);

        var neutralMin = CustomOptionHolder.neutralRolesCountMin.getSelection();
        var neutralMax = CustomOptionHolder.neutralRolesCountMax.getSelection();
        var impostorNum = TORMapOptions.NumImpostors;

        // Make sure min is less or equal to max
        if (neutralMin > neutralMax) neutralMin = neutralMax;

        // Get the maximum allowed count of each role type based on the minimum and maximum option
        int neutralCountSettings = rnd.Next(neutralMin, neutralMax + 1);
        var crewCountSettings = PlayerControl.AllPlayerControls.Count - neutralCountSettings - impostorNum;

        // Potentially lower the actual maximum to the assignable players
        int maxCrewmateRoles = Mathf.Min(crewmates.Count, crewCountSettings);
        int maxNeutralRoles = Mathf.Min(crewmates.Count, neutralCountSettings);
        int maxImpostorRoles = Mathf.Min(impostors.Count, impostorNum);

        // Fill in the lists with the roles that should be assigned to players. Note that the special roles (like Mafia or Lovers) are NOT included in these lists
        Dictionary<byte, int> impSettings = new();
        Dictionary<byte, int> neutralSettings = new();
        Dictionary<byte, int> crewSettings = new();

        impSettings.Add((byte)RoleId.Morphling, CustomOptionHolder.morphlingSpawnRate.getSelection());
        impSettings.Add((byte)RoleId.Camouflager, CustomOptionHolder.camouflagerSpawnRate.getSelection());
        impSettings.Add((byte)RoleId.Vampire, CustomOptionHolder.vampireSpawnRate.getSelection());
        impSettings.Add((byte)RoleId.Eraser, CustomOptionHolder.eraserSpawnRate.getSelection());
        impSettings.Add((byte)RoleId.Trickster, CustomOptionHolder.tricksterSpawnRate.getSelection());
        impSettings.Add((byte)RoleId.Cleaner, CustomOptionHolder.cleanerSpawnRate.getSelection());
        impSettings.Add((byte)RoleId.Undertaker, CustomOptionHolder.undertakerSpawnRate.getSelection());
        impSettings.Add((byte)RoleId.Miner, CustomOptionHolder.minerSpawnRate.getSelection());
        impSettings.Add((byte)RoleId.Warlock, CustomOptionHolder.warlockSpawnRate.getSelection());
        impSettings.Add((byte)RoleId.BountyHunter, CustomOptionHolder.bountyHunterSpawnRate.getSelection());
        impSettings.Add((byte)RoleId.Witch, CustomOptionHolder.witchSpawnRate.getSelection());
        impSettings.Add((byte)RoleId.Escapist, CustomOptionHolder.escapistSpawnRate.getSelection());
        impSettings.Add((byte)RoleId.Ninja, CustomOptionHolder.ninjaSpawnRate.getSelection());
        impSettings.Add((byte)RoleId.Poucher, CustomOptionHolder.poucherSpawnRate.getSelection());
        impSettings.Add((byte)RoleId.Mimic, CustomOptionHolder.mimicSpawnRate.getSelection());
        impSettings.Add((byte)RoleId.Terrorist, CustomOptionHolder.terroristSpawnRate.getSelection());
        impSettings.Add((byte)RoleId.Bomber, CustomOptionHolder.bomberSpawnRate.getSelection());
        impSettings.Add((byte)RoleId.Blackmailer, CustomOptionHolder.blackmailerSpawnRate.getSelection());
        impSettings.Add((byte)RoleId.Yoyo, CustomOptionHolder.yoyoSpawnRate.getSelection());

        neutralSettings.Add((byte)RoleId.Jester, CustomOptionHolder.jesterSpawnRate.getSelection());
        neutralSettings.Add((byte)RoleId.Amnisiac, CustomOptionHolder.amnisiacSpawnRate.getSelection());
        neutralSettings.Add((byte)RoleId.Arsonist, CustomOptionHolder.arsonistSpawnRate.getSelection());
        neutralSettings.Add((byte)RoleId.Jackal, CustomOptionHolder.jackalSpawnRate.getSelection());
        neutralSettings.Add((byte)RoleId.Werewolf, CustomOptionHolder.werewolfSpawnRate.getSelection());
        neutralSettings.Add((byte)RoleId.Vulture, CustomOptionHolder.vultureSpawnRate.getSelection());
        neutralSettings.Add((byte)RoleId.Thief, CustomOptionHolder.thiefSpawnRate.getSelection());
        neutralSettings.Add((byte)RoleId.Doomsayer, CustomOptionHolder.doomsayerSpawnRate.getSelection());
        neutralSettings.Add((byte)RoleId.Juggernaut, CustomOptionHolder.juggernautSpawnRate.getSelection());

        if (rnd.Next(1, 101) <= CustomOptionHolder.lawyerIsProsecutorChance.getSelection() * 10) // Lawyer or Prosecutor
            neutralSettings.Add((byte)RoleId.Prosecutor, CustomOptionHolder.lawyerSpawnRate.getSelection());
        else
            neutralSettings.Add((byte)RoleId.Lawyer, CustomOptionHolder.lawyerSpawnRate.getSelection());

        crewSettings.Add((byte)RoleId.Mayor, CustomOptionHolder.mayorSpawnRate.getSelection());
        crewSettings.Add((byte)RoleId.Portalmaker, CustomOptionHolder.portalmakerSpawnRate.getSelection());
        crewSettings.Add((byte)RoleId.Engineer, CustomOptionHolder.engineerSpawnRate.getSelection());
        crewSettings.Add((byte)RoleId.PrivateInvestigator, CustomOptionHolder.privateInvestigatorSpawnRate.getSelection());
        crewSettings.Add((byte)RoleId.Lighter, CustomOptionHolder.lighterSpawnRate.getSelection());
        crewSettings.Add((byte)RoleId.BodyGuard, CustomOptionHolder.bodyGuardSpawnRate.getSelection());
        crewSettings.Add((byte)RoleId.Detective, CustomOptionHolder.detectiveSpawnRate.getSelection());
        crewSettings.Add((byte)RoleId.TimeMaster, CustomOptionHolder.timeMasterSpawnRate.getSelection());
        crewSettings.Add((byte)RoleId.Veteren, CustomOptionHolder.veterenSpawnRate.getSelection());
        crewSettings.Add((byte)RoleId.Medic, CustomOptionHolder.medicSpawnRate.getSelection());
        crewSettings.Add((byte)RoleId.Swapper, CustomOptionHolder.swapperSpawnRate.getSelection());
        crewSettings.Add((byte)RoleId.Seer, CustomOptionHolder.seerSpawnRate.getSelection());
        crewSettings.Add((byte)RoleId.Hacker, CustomOptionHolder.hackerSpawnRate.getSelection());
        crewSettings.Add((byte)RoleId.Tracker, CustomOptionHolder.trackerSpawnRate.getSelection());
        crewSettings.Add((byte)RoleId.Snitch, CustomOptionHolder.snitchSpawnRate.getSelection());
        crewSettings.Add((byte)RoleId.Medium, CustomOptionHolder.mediumSpawnRate.getSelection());
        if (!isGuesserGamemode) crewSettings.Add((byte)RoleId.NiceGuesser, CustomOptionHolder.guesserSpawnRate.getSelection());
        crewSettings.Add((byte)RoleId.Trapper, CustomOptionHolder.trapperSpawnRate.getSelection());
        if (impostors.Count > 1)
        {
            // Only add Spy if more than 1 impostor as the spy role is otherwise useless
            crewSettings.Add((byte)RoleId.Spy, CustomOptionHolder.spySpawnRate.getSelection());
        }
        crewSettings.Add((byte)RoleId.SecurityGuard, CustomOptionHolder.securityGuardSpawnRate.getSelection());
        crewSettings.Add((byte)RoleId.Jumper, CustomOptionHolder.jumperSpawnRate.getSelection());

        return new RoleAssignmentData
        {
            crewmates = crewmates,
            impostors = impostors,
            crewSettings = crewSettings,
            neutralSettings = neutralSettings,
            impSettings = impSettings,
            maxCrewmateRoles = maxCrewmateRoles,
            maxNeutralRoles = maxNeutralRoles,
            maxImpostorRoles = maxImpostorRoles
        };
    }

    private static void assignSpecialRoles(RoleAssignmentData data)
    {
        // //Assign Cultist
        if (Cultist.isCultistGame)
        {
            setRoleToRandomPlayer((byte)RoleId.Cultist, data.impostors);
        }
        if (data.impostors.Count < 2 && data.maxImpostorRoles < 2 && (rnd.Next(1, 101) <= CustomOptionHolder.cultistSpawnRate.getSelection() * 10))
        {
            //var index = rnd.Next(0, data.impostors.Count);
            //PlayerControl playerControl = data.impostors[index];

            //Helpers.turnToCrewmate(playerControl);

            //data.impostors.RemoveAt(index);
            //data.crewmates.Add(playerControl);
            //setRoleToRandomPlayer((byte)RoleId.Cultist, data.impostors);
            //data.impostors.Count = 1;
            data.impostors.Capacity = 1;
            data.maxImpostorRoles = 1;


        }
        // Assign Mafia
        if (data.impostors.Count >= 3 && data.maxImpostorRoles >= 3 && (rnd.Next(1, 101) <= CustomOptionHolder.mafiaSpawnRate.getSelection() * 10))
        {
            setRoleToRandomPlayer((byte)RoleId.Godfather, data.impostors);
            setRoleToRandomPlayer((byte)RoleId.Janitor, data.impostors);
            setRoleToRandomPlayer((byte)RoleId.Mafioso, data.impostors);
            data.maxImpostorRoles -= 3;
        }
    }

    private static void selectFactionForFactionIndependentRoles(RoleAssignmentData data)
    {
        /*
        if (!isGuesserGamemode) {
            // Assign Guesser (chance to be impostor based on setting)
            isEvilGuesser = rnd.Next(1, 101) <= CustomOptionHolder.guesserIsImpGuesserRate.getSelection() * 10;
            if ((CustomOptionHolder.guesserSpawnBothRate.getSelection() > 0 &&
                CustomOptionHolder.guesserSpawnRate.getSelection() == 10) ||
                CustomOptionHolder.guesserSpawnBothRate.getSelection() == 0) {
                if (isEvilGuesser) data.impSettings.Add((byte)RoleId.EvilGuesser, CustomOptionHolder.guesserSpawnRate.getSelection());
                else data.crewSettings.Add((byte)RoleId.NiceGuesser, CustomOptionHolder.guesserSpawnRate.getSelection());

            }
        }
        */

        // Assign Sheriff
        if ((CustomOptionHolder.deputySpawnRate.getSelection() > 0 &&
            CustomOptionHolder.sheriffSpawnRate.getSelection() == 10) ||
            CustomOptionHolder.deputySpawnRate.getSelection() == 0)
            data.crewSettings.Add((byte)RoleId.Sheriff, CustomOptionHolder.sheriffSpawnRate.getSelection());


        crewValues = data.crewSettings.Values.ToList().Sum();
        impValues = data.impSettings.Values.ToList().Sum();
    }

    private static void assignEnsuredRoles(RoleAssignmentData data)
    {
        // Get all roles where the chance to occur is set to 100%
        List<byte> ensuredCrewmateRoles = data.crewSettings.Where(x => x.Value == 10).Select(x => x.Key).ToList();
        List<byte> ensuredNeutralRoles = data.neutralSettings.Where(x => x.Value == 10).Select(x => x.Key).ToList();
        List<byte> ensuredImpostorRoles = data.impSettings.Where(x => x.Value == 10).Select(x => x.Key).ToList();

        // Assign roles until we run out of either players we can assign roles to or run out of roles we can assign to players
        while (
            (data.impostors.Count > 0 && data.maxImpostorRoles > 0 && ensuredImpostorRoles.Count > 0) ||
            (data.crewmates.Count > 0 && (
                (data.maxCrewmateRoles > 0 && ensuredCrewmateRoles.Count > 0) ||
                (data.maxNeutralRoles > 0 && ensuredNeutralRoles.Count > 0)
            )))
        {

            Dictionary<RoleType, List<byte>> rolesToAssign = new();
            if (data.crewmates.Count > 0 && data.maxCrewmateRoles > 0 && ensuredCrewmateRoles.Count > 0) rolesToAssign.Add(RoleType.Crewmate, ensuredCrewmateRoles);
            if (data.crewmates.Count > 0 && data.maxNeutralRoles > 0 && ensuredNeutralRoles.Count > 0) rolesToAssign.Add(RoleType.Neutral, ensuredNeutralRoles);
            if (data.impostors.Count > 0 && data.maxImpostorRoles > 0 && ensuredImpostorRoles.Count > 0) rolesToAssign.Add(RoleType.Impostor, ensuredImpostorRoles);

            // Randomly select a pool of roles to assign a role from next (Crewmate role, Neutral role or Impostor role) 
            // then select one of the roles from the selected pool to a player 
            // and remove the role (and any potentially blocked role pairings) from the pool(s)
            var roleType = rolesToAssign.Keys.ElementAt(rnd.Next(0, rolesToAssign.Keys.Count()));
            var players = roleType == RoleType.Crewmate || roleType == RoleType.Neutral ? data.crewmates : data.impostors;
            var index = rnd.Next(0, rolesToAssign[roleType].Count);
            var roleId = rolesToAssign[roleType][index];
            setRoleToRandomPlayer(rolesToAssign[roleType][index], players);
            rolesToAssign[roleType].RemoveAt(index);

            if (CustomOptionHolder.blockedRolePairings.ContainsKey(roleId))
            {
                foreach (var blockedRoleId in CustomOptionHolder.blockedRolePairings[roleId])
                {
                    // Set chance for the blocked roles to 0 for chances less than 100%
                    if (data.impSettings.ContainsKey(blockedRoleId)) data.impSettings[blockedRoleId] = 0;
                    if (data.neutralSettings.ContainsKey(blockedRoleId)) data.neutralSettings[blockedRoleId] = 0;
                    if (data.crewSettings.ContainsKey(blockedRoleId)) data.crewSettings[blockedRoleId] = 0;
                    // Remove blocked roles even if the chance was 100%
                    foreach (var ensuredRolesList in rolesToAssign.Values)
                    {
                        ensuredRolesList.RemoveAll(x => x == blockedRoleId);
                    }
                }
            }

            // Adjust the role limit
            switch (roleType)
            {
                case RoleType.Crewmate: data.maxCrewmateRoles--; crewValues -= 10; break;
                case RoleType.Neutral: data.maxNeutralRoles--; break;
                case RoleType.Impostor: data.maxImpostorRoles--; impValues -= 10; break;
            }
        }
    }

    private static void assignDependentRoles(RoleAssignmentData data)
    {
        // Roles that prob have a dependent role
        //bool guesserFlag = CustomOptionHolder.guesserSpawnBothRate.getSelection() > 0 
        //     && CustomOptionHolder.guesserSpawnRate.getSelection() > 0;
        bool sheriffFlag = CustomOptionHolder.deputySpawnRate.getSelection() > 0
            && CustomOptionHolder.sheriffSpawnRate.getSelection() > 0;

        //if (isGuesserGamemode) guesserFlag = false;
        // if (!guesserFlag && !sheriffFlag) return; // assignDependentRoles is not needed

        int crew = data.crewmates.Count < data.maxCrewmateRoles ? data.crewmates.Count : data.maxCrewmateRoles; // Max number of crew loops
        int imp = data.impostors.Count < data.maxImpostorRoles ? data.impostors.Count : data.maxImpostorRoles; // Max number of imp loops
        int crewSteps = crew / data.crewSettings.Keys.Count(); // Avarage crewvalues deducted after each loop 
        int impSteps = imp / data.impSettings.Keys.Count(); // Avarage impvalues deducted after each loop

        // set to false if needed, otherwise we can skip the loop
        bool isSheriff = !sheriffFlag;
        //bool isGuesser = !guesserFlag;

        // --- Simulate Crew & Imp ticket system ---
        while (crew > 0 && (!isSheriff /* || (!isEvilGuesser && !isGuesser)*/))
        {
            if (!isSheriff && rnd.Next(crewValues) < CustomOptionHolder.sheriffSpawnRate.getSelection()) isSheriff = true;
            //if (!isEvilGuesser && !isGuesser && rnd.Next(crewValues) < CustomOptionHolder.guesserSpawnRate.getSelection()) isGuesser = true;
            crew--;
            crewValues -= crewSteps;
        }
        /*
        while (imp > 0 && (isEvilGuesser && !isGuesser)) { 
            if (rnd.Next(impValues) < CustomOptionHolder.guesserSpawnRate.getSelection()) isGuesser = true;
            imp--;
            impValues -= impSteps;
        }
        */

        // --- Assign Main Roles if they won the lottery ---
        if (isSheriff && Sheriff.sheriff == null && data.crewmates.Count > 0 && data.maxCrewmateRoles > 0 && sheriffFlag)
        { // Set Sheriff cause he won the lottery
            byte sheriff = setRoleToRandomPlayer((byte)RoleId.Sheriff, data.crewmates);
            data.crewmates.ToList().RemoveAll(x => x.PlayerId == sheriff);
            data.maxCrewmateRoles--;
        }
        /*
        if (!isGuesserGamemode) {
            if (!isEvilGuesser && isGuesser && Guesser.niceGuesser == null && data.crewmates.Count > 0 && data.maxCrewmateRoles > 0 && guesserFlag) { // Set Nice Guesser cause he won the lottery
                byte niceGuesser = setRoleToRandomPlayer((byte)RoleId.NiceGuesser, data.crewmates);
                data.crewmates.ToList().RemoveAll(x => x.PlayerId == niceGuesser);
                data.maxCrewmateRoles--;
            }
            else if (isEvilGuesser && isGuesser && Guesser.evilGuesser == null && data.impostors.Count > 0 && data.maxImpostorRoles > 0 && guesserFlag) { // Set Evil Guesser cause he won the lottery
                byte evilGuesser = setRoleToRandomPlayer((byte)RoleId.EvilGuesser, data.impostors);
                data.impostors.ToList().RemoveAll(x => x.PlayerId == evilGuesser);
                data.maxImpostorRoles--;
            }
        }
        */

        // --- Assign Dependent Roles if main role exists ---
        if (Sheriff.sheriff != null)
        { // Deputy
            if (CustomOptionHolder.deputySpawnRate.getSelection() == 10 && data.crewmates.Count > 0 && data.maxCrewmateRoles > 0)
            { // Force Deputy
                byte deputy = setRoleToRandomPlayer((byte)RoleId.Deputy, data.crewmates);
                data.crewmates.ToList().RemoveAll(x => x.PlayerId == deputy);
                data.maxCrewmateRoles--;
            }
            else if (CustomOptionHolder.deputySpawnRate.getSelection() < 10) // Dont force, add Deputy to the ticket system
                data.crewSettings.Add((byte)RoleId.Deputy, CustomOptionHolder.deputySpawnRate.getSelection());
        }

        if (!data.crewSettings.ContainsKey((byte)RoleId.Sheriff)) data.crewSettings.Add((byte)RoleId.Sheriff, 0);
        /*
                    if (!isGuesserGamemode) {
                        if (!isEvilGuesser && Guesser.niceGuesser != null) { // Other Guesser (evil)
                            if (CustomOptionHolder.guesserSpawnBothRate.getSelection() == 10 && data.impostors.Count > 0 && data.maxImpostorRoles > 0) { // Force other guesser (evil)
                                byte bothGuesser = setRoleToRandomPlayer((byte)RoleId.EvilGuesser, data.impostors);
                                data.impostors.ToList().RemoveAll(x => x.PlayerId == bothGuesser);
                                data.maxImpostorRoles--;
                            }
                            else if (CustomOptionHolder.guesserSpawnBothRate.getSelection() < 10) // Dont force, add Guesser (evil) to the ticket system
                                data.impSettings.Add((byte)RoleId.EvilGuesser, CustomOptionHolder.guesserSpawnBothRate.getSelection());
                        }
                        else if (isEvilGuesser && Guesser.evilGuesser != null) { // ELSE other Guesser (nice)
                            if (CustomOptionHolder.guesserSpawnBothRate.getSelection() == 10 && data.crewmates.Count > 0 && data.maxCrewmateRoles > 0) { // Force other guesser (nice)
                                byte bothGuesser = setRoleToRandomPlayer((byte)RoleId.NiceGuesser, data.crewmates);
                                data.crewmates.ToList().RemoveAll(x => x.PlayerId == bothGuesser);
                                data.maxCrewmateRoles--;
                            }
                            else if (CustomOptionHolder.guesserSpawnBothRate.getSelection() < 10) // Dont force, add Guesser (nice) to the ticket system
                                data.crewSettings.Add((byte)RoleId.NiceGuesser, CustomOptionHolder.guesserSpawnBothRate.getSelection());
                        }
                    }
                    */
    }
    private static void assignChanceRoles(RoleAssignmentData data)
    {
        // Get all roles where the chance to occur is set grater than 0% but not 100% and build a ticket pool based on their weight
        List<byte> crewmateTickets = data.crewSettings.Where(x => x.Value > 0 && x.Value < 10).Select(x => Enumerable.Repeat(x.Key, x.Value)).SelectMany(x => x).ToList();
        List<byte> neutralTickets = data.neutralSettings.Where(x => x.Value > 0 && x.Value < 10).Select(x => Enumerable.Repeat(x.Key, x.Value)).SelectMany(x => x).ToList();
        List<byte> impostorTickets = data.impSettings.Where(x => x.Value > 0 && x.Value < 10).Select(x => Enumerable.Repeat(x.Key, x.Value)).SelectMany(x => x).ToList();

        // Assign roles until we run out of either players we can assign roles to or run out of roles we can assign to players
        while (
            (data.impostors.Count > 0 && data.maxImpostorRoles > 0 && impostorTickets.Count > 0) ||
            (data.crewmates.Count > 0 && (
                (data.maxCrewmateRoles > 0 && crewmateTickets.Count > 0) ||
                (data.maxNeutralRoles > 0 && neutralTickets.Count > 0)
            )))
        {

            Dictionary<RoleType, List<byte>> rolesToAssign = new();
            if (data.crewmates.Count > 0 && data.maxCrewmateRoles > 0 && crewmateTickets.Count > 0) rolesToAssign.Add(RoleType.Crewmate, crewmateTickets);
            if (data.crewmates.Count > 0 && data.maxNeutralRoles > 0 && neutralTickets.Count > 0) rolesToAssign.Add(RoleType.Neutral, neutralTickets);
            if (data.impostors.Count > 0 && data.maxImpostorRoles > 0 && impostorTickets.Count > 0) rolesToAssign.Add(RoleType.Impostor, impostorTickets);

            // Randomly select a pool of role tickets to assign a role from next (Crewmate role, Neutral role or Impostor role) 
            // then select one of the roles from the selected pool to a player 
            // and remove all tickets of this role (and any potentially blocked role pairings) from the pool(s)
            var roleType = rolesToAssign.Keys.ElementAt(rnd.Next(0, rolesToAssign.Keys.Count()));
            var players = roleType == RoleType.Crewmate || roleType == RoleType.Neutral ? data.crewmates : data.impostors;
            var index = rnd.Next(0, rolesToAssign[roleType].Count);
            var roleId = rolesToAssign[roleType][index];
            setRoleToRandomPlayer(roleId, players);
            rolesToAssign[roleType].RemoveAll(x => x == roleId);

            if (CustomOptionHolder.blockedRolePairings.ContainsKey(roleId))
            {
                foreach (var blockedRoleId in CustomOptionHolder.blockedRolePairings[roleId])
                {
                    // Remove tickets of blocked roles from all pools
                    crewmateTickets.RemoveAll(x => x == blockedRoleId);
                    neutralTickets.RemoveAll(x => x == blockedRoleId);
                    impostorTickets.RemoveAll(x => x == blockedRoleId);
                }
            }

            // Adjust the role limit
            switch (roleType)
            {
                case RoleType.Crewmate: data.maxCrewmateRoles--; break;
                case RoleType.Neutral: data.maxNeutralRoles--; break;
                case RoleType.Impostor: data.maxImpostorRoles--; break;
            }
        }
    }

    private static void assignRoleTargets(RoleAssignmentData data)
    {
        // Set Lawyer or Prosecutor Target
        if (Lawyer.lawyer != null)
        {
            var possibleTargets = new List<PlayerControl>();
            if (!Lawyer.isProsecutor)
            { // Lawyer
                foreach (PlayerControl p in CachedPlayer.AllPlayers)
                {
                    if (!p.Data.IsDead && !p.Data.Disconnected &&
                        p != Lovers.lover1 && p != Lovers.lover2 && (p.Data.Role.IsImpostor || p == Jackal.jackal || p == Juggernaut.juggernaut || p == Werewolf.werewolf || (Lawyer.targetCanBeJester && p == Jester.jester)))
                        possibleTargets.Add(p);
                }
            }
            else
            { // Prosecutor
                foreach (PlayerControl p in CachedPlayer.AllPlayers)
                {
                    if (!p.Data.IsDead && !p.Data.Disconnected && p != Lovers.lover1 && p != Lovers.lover2 && p != Mini.mini && !p.Data.Role.IsImpostor && !isNeutral(p) && p != Swapper.swapper)
                        possibleTargets.Add(p);
                }
            }

            if (possibleTargets.Count == 0)
            {
                MessageWriter w = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.LawyerPromotesToPursuer, SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(w);
                RPCProcedure.lawyerPromotesToPursuer();
            }
            else
            {
                var target = possibleTargets[rnd.Next(0, possibleTargets.Count)];
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.LawyerSetTarget, SendOption.Reliable, -1);
                writer.Write(target.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.lawyerSetTarget(target.PlayerId);
            }
        }
    }

    private static void assignModifiers()
    {
        var modifierMin = CustomOptionHolder.modifiersCountMin.getSelection();
        var modifierMax = CustomOptionHolder.modifiersCountMax.getSelection();
        if (modifierMin > modifierMax) modifierMin = modifierMax;
        int modifierCountSettings = rnd.Next(modifierMin, modifierMax + 1);
        List<PlayerControl> players = PlayerControl.AllPlayerControls.ToArray().ToList();
        if (isGuesserGamemode && !CustomOptionHolder.guesserGamemodeHaveModifier.getBool())
            players.RemoveAll(x => GuesserGM.isGuesser(x.PlayerId));

        List<PlayerControl> impPlayer = new(players);
        //List<PlayerControl> neutralPlayer = new List<PlayerControl>(players);
        List<PlayerControl> impPlayerL = new(players);
        List<PlayerControl> crewPlayer = new(players);
        impPlayer.RemoveAll(x => !x.Data.Role.IsImpostor);
        //neutralPlayer.RemoveAll(x => !Helpers.isNeutral(x));
        impPlayerL.RemoveAll(x => !x.Data.Role.IsImpostor);
        crewPlayer.RemoveAll(x => x.Data.Role.IsImpostor);

        int modifierCount = Mathf.Min(players.Count, modifierCountSettings);

        if (modifierCount == 0) return;

        List<RoleId> allModifiers = new();
        List<RoleId> ensuredModifiers = new();
        List<RoleId> chanceModifiers = new();

        List<RoleId> impModifiers = new();
        List<RoleId> ensuredImpModifiers = new();
        List<RoleId> chanceImpModifiers = new();
        allModifiers.AddRange(new List<RoleId>
        {
            RoleId.Aftermath,
            RoleId.Tiebreaker,
            RoleId.Mini,
            RoleId.Giant,
            RoleId.Bait,
            RoleId.Bloody,
            RoleId.AntiTeleport,
            RoleId.Sunglasses,
            RoleId.Torch,
            RoleId.Flash,
            RoleId.Multitasker,
            RoleId.Vip,
            RoleId.Invert,
            RoleId.Indomitable,
            RoleId.Tunneler,
            RoleId.Slueth,
            RoleId.Blind,
            RoleId.Watcher,
            RoleId.Radar,
            RoleId.Disperser,
            RoleId.Cursed,
            RoleId.Chameleon,
            RoleId.Shifter
        });

        impModifiers.AddRange(new List<RoleId>
        {
            RoleId.EvilGuesser
        });

        if (rnd.Next(1, 101) <= CustomOptionHolder.modifierLover.getSelection() * 10)
        { // Assign lover
            bool isEvilLover = rnd.Next(1, 101) <= CustomOptionHolder.modifierLoverImpLoverRate.getSelection() * 10;
            byte firstLoverId;
            //List<PlayerControl> impPlayer = new List<PlayerControl>(players);
            //List<PlayerControl> impPlayerL = new List<PlayerControl>(players);
            //List<PlayerControl> crewPlayer = new List<PlayerControl>(players);
            impPlayer.RemoveAll(x => !x.Data.Role.IsImpostor);
            impPlayerL.RemoveAll(x => !x.Data.Role.IsImpostor);
            crewPlayer.RemoveAll(x => x.Data.Role.IsImpostor || x == Lawyer.lawyer);

            if (!Cultist.isCultistGame)
            {
                if (isEvilLover) firstLoverId = setModifierToRandomPlayer((byte)RoleId.Lover, impPlayerL);
                else firstLoverId = setModifierToRandomPlayer((byte)RoleId.Lover, crewPlayer);
                byte secondLoverId = setModifierToRandomPlayer((byte)RoleId.Lover, crewPlayer, 1);

                players.RemoveAll(x => x.PlayerId == firstLoverId || x.PlayerId == secondLoverId);
                modifierCount--;
            }

            if (Cultist.isCultistGame)
            {
                firstLoverId = setModifierToRandomPlayer((byte)RoleId.Lover, crewPlayer);
                byte secondLoverId = setModifierToRandomPlayer((byte)RoleId.Lover, crewPlayer, 1);


                players.RemoveAll(x => x.PlayerId == firstLoverId || x.PlayerId == secondLoverId);
                modifierCount--;
            }
        }

        foreach (RoleId m in allModifiers)
        {
            if (getSelectionForRoleId(m) == 10) ensuredModifiers.AddRange(Enumerable.Repeat(m, getSelectionForRoleId(m, true) / 10));
            else chanceModifiers.AddRange(Enumerable.Repeat(m, getSelectionForRoleId(m, true)));
        }
        foreach (RoleId m in impModifiers)
        {
            if (getSelectionForRoleId(m) == 10) ensuredImpModifiers.AddRange(Enumerable.Repeat(m, getSelectionForRoleId(m, true) / 10));
            else chanceImpModifiers.AddRange(Enumerable.Repeat(m, getSelectionForRoleId(m, true)));
        }

        assignModifiersToPlayers(ensuredImpModifiers, impPlayer, modifierCount); // Assign ensured imp modifier
        assignModifiersToPlayers(ensuredModifiers, players, modifierCount); // Assign ensured modifier

        modifierCount -= ensuredImpModifiers.Count + ensuredModifiers.Count;
        if (modifierCount <= 0) return;
        int chanceModifierCount = Mathf.Min(modifierCount, chanceModifiers.Count);
        List<RoleId> chanceModifierToAssign = new();
        while (chanceModifierCount > 0 && chanceModifiers.Count > 0)
        {
            var index = rnd.Next(0, chanceModifiers.Count);
            RoleId modifierId = chanceModifiers[index];
            chanceModifierToAssign.Add(modifierId);

            int modifierSelection = getSelectionForRoleId(modifierId);
            while (modifierSelection > 0)
            {
                chanceModifiers.Remove(modifierId);
                modifierSelection--;
            }
            chanceModifierCount--;
        }

        assignModifiersToPlayers(chanceModifierToAssign, players, modifierCount); // Assign chance modifier

        int chanceImpModifierCount = Mathf.Min(modifierCount, chanceImpModifiers.Count);
        List<RoleId> chanceImpModifierToAssign = new();
        while (chanceImpModifierCount > 0 && chanceImpModifiers.Count > 0)
        {
            var index = rnd.Next(0, chanceImpModifiers.Count);
            RoleId modifierId = chanceImpModifiers[index];
            chanceImpModifierToAssign.Add(modifierId);

            int modifierSelection = getSelectionForRoleId(modifierId);
            while (modifierSelection > 0)
            {
                chanceImpModifiers.Remove(modifierId);
                modifierSelection--;
            }
            chanceImpModifierCount--;
        }
        assignModifiersToPlayers(chanceImpModifierToAssign, impPlayer, modifierCount); // Assign chance Imp modifier
    }

    private static void assignGuesserGamemode()
    {
        var impPlayer = PlayerControl.AllPlayerControls.ToArray().ToList().OrderBy(x => Guid.NewGuid()).ToList();
        var neutralPlayer = PlayerControl.AllPlayerControls.ToArray().ToList().OrderBy(x => Guid.NewGuid()).ToList();
        var crewPlayer = PlayerControl.AllPlayerControls.ToArray().ToList().OrderBy(x => Guid.NewGuid()).ToList();
        impPlayer.RemoveAll(x => !x.Data.Role.IsImpostor);
        neutralPlayer.RemoveAll(x => !isNeutral(x));
        neutralPlayer.RemoveAll(x => x == Doomsayer.doomsayer);
        crewPlayer.RemoveAll(x => x.Data.Role.IsImpostor || isNeutral(x));
        assignGuesserGamemodeToPlayers(crewPlayer,
            Mathf.RoundToInt(CustomOptionHolder.guesserGamemodeCrewNumber.getFloat()));
        assignGuesserGamemodeToPlayers(neutralPlayer,
            Mathf.RoundToInt(CustomOptionHolder.guesserGamemodeNeutralNumber.getFloat()),
            CustomOptionHolder.guesserForceJackalGuesser.getBool(),
            CustomOptionHolder.guesserForceThiefGuesser.getBool());
        assignGuesserGamemodeToPlayers(impPlayer,
            Mathf.RoundToInt(CustomOptionHolder.guesserGamemodeImpNumber.getFloat()));
    }

    private static void assignGuesserGamemodeToPlayers(List<PlayerControl> playerList, int count, bool forceJackal = false, bool forceThief = false)
    {
        for (int i = 0; i < count && playerList.Count > 0; i++)
        {
            var index = rnd.Next(0, playerList.Count);
            if (forceThief && !forceJackal)
            {
                if (Thief.thief != null)
                    index = playerList.FindIndex(x => x == Thief.thief);
                forceThief = false;
            }
            if (forceJackal)
            {
                if (Jackal.jackal != null)
                    index = playerList.FindIndex(x => x == Jackal.jackal);
                forceJackal = false;
            }
            byte playerId = playerList[index].PlayerId;
            playerList.RemoveAt(index);

            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.SetGuesserGm, SendOption.Reliable, -1);
            writer.Write(playerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.setGuesserGm(playerId);
        }
    }

    private static byte setRoleToRandomPlayer(byte roleId, List<PlayerControl> playerList, bool removePlayer = true)
    {
        var index = rnd.Next(0, playerList.Count);
        byte playerId = playerList[index].PlayerId;
        if (removePlayer) playerList.RemoveAt(index);

        playerRoleMap.Add(new Tuple<byte, byte>(playerId, roleId));

        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetRole, SendOption.Reliable, -1);
        writer.Write(roleId);
        writer.Write(playerId);
        AmongUsClient.Instance.FinishRpcImmediately(writer);
        RPCProcedure.setRole(roleId, playerId);
        return playerId;
    }

    private static byte setModifierToRandomPlayer(byte modifierId, List<PlayerControl> playerList, byte flag = 0)
    {
        if (playerList.Count == 0) return Byte.MaxValue;
        var index = rnd.Next(0, playerList.Count);
        byte playerId = playerList[index].PlayerId;
        playerList.RemoveAt(index);

        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.SetModifier, SendOption.Reliable, -1);
        writer.Write(modifierId);
        writer.Write(playerId);
        writer.Write(flag);
        AmongUsClient.Instance.FinishRpcImmediately(writer);
        RPCProcedure.setModifier(modifierId, playerId, flag);
        return playerId;
    }

    private static void assignModifiersToPlayers(List<RoleId> modifiers, List<PlayerControl> playerList, int modifierCount)
    {
        modifiers = modifiers.OrderBy(x => rnd.Next()).ToList(); // randomize list

        while (modifierCount < modifiers.Count)
        {
            var index = rnd.Next(0, modifiers.Count);
            modifiers.RemoveAt(index);
        }

        byte playerId;

        if (modifiers.Contains(RoleId.EvilGuesser))
        {
            List<PlayerControl> impPlayer = new(playerList); //testing
            impPlayer.RemoveAll(x => !x.Data.Role.IsImpostor);

            int assassinCount = 0;
            while (assassinCount < modifiers.FindAll(x => x == RoleId.EvilGuesser).Count)
            {
                playerId = setModifierToRandomPlayer((byte)RoleId.EvilGuesser, impPlayer);
                //   crewPlayer.RemoveAll(x => x.PlayerId == playerId);
                playerList.RemoveAll(x => x.PlayerId == playerId);
                assassinCount++;
            }
            modifiers.RemoveAll(x => x == RoleId.EvilGuesser);
        }

        if (modifiers.Contains(RoleId.Disperser))
        {
            List<PlayerControl> impPlayer = new(playerList); //testing
            impPlayer.RemoveAll(x => !x.Data.Role.IsImpostor);
            playerId = setModifierToRandomPlayer((byte)RoleId.Disperser, impPlayer);
            //   crewPlayer.RemoveAll(x => x.PlayerId == playerId);
            playerList.RemoveAll(x => x.PlayerId == playerId);
            modifiers.RemoveAll(x => x == RoleId.Disperser);
        }

        if (modifiers.Contains(RoleId.Cursed))
        {
            List<PlayerControl> crewPlayerC = new(playerList);
            crewPlayerC.RemoveAll(x => x.Data.Role.IsImpostor || isNeutral(x));
            playerId = setModifierToRandomPlayer((byte)RoleId.Cursed, crewPlayerC);
            playerList.RemoveAll(x => x.PlayerId == playerId);
            modifiers.RemoveAll(x => x == RoleId.Cursed);
        }

        if (modifiers.Contains(RoleId.Tunneler))
        {
            List<PlayerControl> crewPlayerT = new(playerList);
            crewPlayerT.RemoveAll(x => x.Data.Role.IsImpostor || isNeutral(x) || x == Engineer.engineer);
            playerId = setModifierToRandomPlayer((byte)RoleId.Tunneler, crewPlayerT);
            playerList.RemoveAll(x => x.PlayerId == playerId);
            modifiers.RemoveAll(x => x == RoleId.Tunneler);
        }
        /*
        if (modifiers.Contains(RoleId.Watcher)) {
            List<PlayerControl> crewPlayerW = new List<PlayerControl>(playerList);
            crewPlayerW.RemoveAll(x => x.Data.Role.IsImpostor);
            playerId = setModifierToRandomPlayer((byte)RoleId.Watcher, crewPlayerW);
            playerList.RemoveAll(x => x.PlayerId == playerId);
            modifiers.RemoveAll(x => x == RoleId.Watcher);
        }
        */
        List<PlayerControl> crewPlayer = new(playerList);
        crewPlayer.RemoveAll(x => x.Data.Role.IsImpostor || isNeutral(x));
        if (modifiers.Contains(RoleId.Shifter))
        {
            var crewPlayerShifter = new List<PlayerControl>(crewPlayer);
            crewPlayerShifter.RemoveAll(x => x == Spy.spy);
            playerId = setModifierToRandomPlayer((byte)RoleId.Shifter, crewPlayerShifter);
            crewPlayer.RemoveAll(x => x.PlayerId == playerId);
            playerList.RemoveAll(x => x.PlayerId == playerId);
            modifiers.RemoveAll(x => x == RoleId.Shifter);
        }
        if (modifiers.Contains(RoleId.Sunglasses))
        {
            int sunglassesCount = 0;
            while (sunglassesCount < modifiers.FindAll(x => x == RoleId.Sunglasses).Count)
            {
                playerId = setModifierToRandomPlayer((byte)RoleId.Sunglasses, crewPlayer);
                crewPlayer.RemoveAll(x => x.PlayerId == playerId);
                playerList.RemoveAll(x => x.PlayerId == playerId);
                sunglassesCount++;
            }
            modifiers.RemoveAll(x => x == RoleId.Sunglasses);
        }

        if (modifiers.Contains(RoleId.Aftermath))
        {
            var Player = new List<PlayerControl>(playerList);
            Player.RemoveAll(x => x.Data.Role.IsImpostor);
            playerId = setModifierToRandomPlayer((byte)RoleId.Aftermath, Player);
            Player.RemoveAll(x => x.PlayerId == playerId);
            playerList.RemoveAll(x => x.PlayerId == playerId);
            modifiers.RemoveAll(x => x == RoleId.Aftermath);
        }

        if (modifiers.Contains(RoleId.Torch))
        {
            int torchCount = 0;
            while (torchCount < modifiers.FindAll(x => x == RoleId.Torch).Count)
            {
                playerId = setModifierToRandomPlayer((byte)RoleId.Torch, crewPlayer);
                crewPlayer.RemoveAll(x => x.PlayerId == playerId);
                playerList.RemoveAll(x => x.PlayerId == playerId);
                torchCount++;
            }
            modifiers.RemoveAll(x => x == RoleId.Torch);
        }

        if (modifiers.Contains(RoleId.Multitasker))
        {
            int multitaskerCount = 0;
            while (multitaskerCount < modifiers.FindAll(x => x == RoleId.Multitasker).Count)
            {
                playerId = setModifierToRandomPlayer((byte)RoleId.Multitasker, crewPlayer);
                crewPlayer.RemoveAll(x => x.PlayerId == playerId);
                playerList.RemoveAll(x => x.PlayerId == playerId);
                multitaskerCount++;
            }
            modifiers.RemoveAll(x => x == RoleId.Multitasker);
        }

        foreach (RoleId modifier in modifiers)
        {
            if (playerList.Count == 0) break;
            playerId = setModifierToRandomPlayer((byte)modifier, playerList);
            playerList.RemoveAll(x => x.PlayerId == playerId);
        }
    }

    private static int getSelectionForRoleId(RoleId roleId, bool multiplyQuantity = false)
    {
        int selection = 0;
        switch (roleId)
        {
            case RoleId.Lover:
                selection = CustomOptionHolder.modifierLover.getSelection(); break;
            case RoleId.Tiebreaker:
                selection = CustomOptionHolder.modifierTieBreaker.getSelection(); break;
            case RoleId.Indomitable:
                selection = CustomOptionHolder.modifierIndomitable.getSelection(); break;
            case RoleId.Cursed:
                selection = CustomOptionHolder.modifierCursed.getSelection(); break;
            case RoleId.Slueth:
                selection = CustomOptionHolder.modifierSlueth.getSelection(); break;
            case RoleId.Blind:
                selection = CustomOptionHolder.modifierBlind.getSelection(); break;
            case RoleId.Watcher:
                selection = CustomOptionHolder.modifierWatcher.getSelection(); break;
            case RoleId.Radar:
                selection = CustomOptionHolder.modifierRadar.getSelection(); break;
            case RoleId.Aftermath:
                selection = CustomOptionHolder.modifierAftermath.getSelection();
                break;
            case RoleId.Disperser:
                selection = CustomOptionHolder.modifierDisperser.getSelection(); break;
            case RoleId.Mini:
                selection = CustomOptionHolder.modifierMini.getSelection(); break;
            case RoleId.Bait:
                selection = CustomOptionHolder.modifierBait.getSelection();
                if (multiplyQuantity) selection *= CustomOptionHolder.modifierBaitQuantity.getQuantity();
                break;
            case RoleId.Bloody:
                selection = CustomOptionHolder.modifierBloody.getSelection();
                if (multiplyQuantity) selection *= CustomOptionHolder.modifierBloodyQuantity.getQuantity();
                break;
            case RoleId.AntiTeleport:
                selection = CustomOptionHolder.modifierAntiTeleport.getSelection();
                if (multiplyQuantity) selection *= CustomOptionHolder.modifierAntiTeleportQuantity.getQuantity();
                break;
            case RoleId.Tunneler:
                selection = CustomOptionHolder.modifierTunneler.getSelection(); break;
            case RoleId.Sunglasses:
                selection = CustomOptionHolder.modifierSunglasses.getSelection();
                if (multiplyQuantity) selection *= CustomOptionHolder.modifierSunglassesQuantity.getQuantity();
                break;
            case RoleId.Torch:
                selection = CustomOptionHolder.modifierTorch.getSelection();
                if (multiplyQuantity) selection *= CustomOptionHolder.modifierTorchQuantity.getQuantity();
                break;
            case RoleId.Flash:
                selection = CustomOptionHolder.modifierFlash.getSelection();
                if (multiplyQuantity) selection *= CustomOptionHolder.modifierFlashQuantity.getQuantity();
                break;
            case RoleId.Multitasker:
                selection = CustomOptionHolder.modifierMultitasker.getSelection();
                if (multiplyQuantity) selection *= CustomOptionHolder.modifierMultitaskerQuantity.getQuantity();
                break;
            case RoleId.Giant:
                selection = CustomOptionHolder.modifierGiant.getSelection();
                break;
            case RoleId.Vip:
                selection = CustomOptionHolder.modifierVip.getSelection();
                if (multiplyQuantity) selection *= CustomOptionHolder.modifierVipQuantity.getQuantity();
                break;
            case RoleId.Invert:
                selection = CustomOptionHolder.modifierInvert.getSelection();
                if (multiplyQuantity) selection *= CustomOptionHolder.modifierInvertQuantity.getQuantity();
                break;
            case RoleId.Chameleon:
                selection = CustomOptionHolder.modifierChameleon.getSelection();
                if (multiplyQuantity) selection *= CustomOptionHolder.modifierChameleonQuantity.getQuantity();
                break;
            case RoleId.Shifter:
                selection = CustomOptionHolder.modifierShifter.getSelection(); break;
            case RoleId.EvilGuesser:
                if (!isGuesserGamemode)
                {
                    selection = CustomOptionHolder.modifierAssassin.getSelection();
                    if (!Cultist.isCultistGame)
                        if (multiplyQuantity)
                            selection *= CustomOptionHolder.modifierAssassinQuantity.getQuantity();
                }
                break;
        }

        return selection;
    }

    private static void setRolesAgain()
    {

        while (playerRoleMap.Any())
        {
            byte amount = (byte)Math.Min(playerRoleMap.Count, 20);
            var writer = AmongUsClient.Instance!.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.WorkaroundSetRoles, SendOption.Reliable, -1);
            writer.Write(amount);
            for (int i = 0; i < amount; i++)
            {
                var option = playerRoleMap[0];
                playerRoleMap.RemoveAt(0);
                writer.WritePacked((uint)option.Item1);
                writer.WritePacked((uint)option.Item2);
            }
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }
    }

    public class RoleAssignmentData
    {
        public List<PlayerControl> crewmates { get; set; }
        public List<PlayerControl> impostors { get; set; }
        public Dictionary<byte, int> impSettings = new();
        public Dictionary<byte, int> neutralSettings = new();
        public Dictionary<byte, int> crewSettings = new();
        public int maxCrewmateRoles { get; set; }
        public int maxNeutralRoles { get; set; }
        public int maxImpostorRoles { get; set; }
    }

    private enum RoleType
    {
        Crewmate = 0,
        Neutral = 1,
        Impostor = 2
    }

}
