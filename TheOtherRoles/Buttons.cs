using System;
using System.Collections.Generic;
using System.Linq;
using Hazel;
using TheOtherRoles.CustomGameModes;
using TheOtherRoles.Objects;
using TheOtherRoles.Patches;
using TheOtherRoles.Utilities;
using UnityEngine;
using static TheOtherRoles.Objects.CustomButton;
using static TheOtherRoles.TheOtherRoles;

namespace TheOtherRoles;

[HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
static class HudManagerStartPatch
{
    private static bool initialized;

    public static CustomButton engineerRepairButton;
    private static CustomButton janitorCleanButton;
    public static CustomButton sheriffKillButton;
    private static CustomButton deputyHandcuffButton;
    public static CustomButton timeMasterShieldButton;
    private static CustomButton amnisiacRememberButton;
    public static CustomButton veterenAlertButton;
    public static CustomButton medicShieldButton;
    public static CustomButton bomberBombButton;
    public static CustomButton bomberGiveButton;
    private static CustomButton cultistTurnButton;
    private static CustomButton shifterShiftButton;
    private static CustomButton disperserDisperseButton;
    public static CustomButton morphlingButton;
    public static CustomButton camouflagerButton;
    public static CustomButton portalmakerPlacePortalButton;
    private static CustomButton usePortalButton;
    private static CustomButton portalmakerMoveToPortalButton;
    public static CustomButton hackerButton;
    //private static CustomButton changeChatButton;
    public static CustomButton hackerVitalsButton;
    public static CustomButton hackerAdminTableButton;
    public static CustomButton trackerTrackPlayerButton;
    public static CustomButton bodyGuardGuardButton;
    public static CustomButton privateInvestigatorWatchButton;
    private static CustomButton trackerTrackCorpsesButton;
    public static CustomButton vampireKillButton;
    public static CustomButton garlicButton;
    public static CustomButton jackalKillButton;
    public static CustomButton sidekickKillButton;
    public static CustomButton swooperSwoopButton;
    private static CustomButton jackalSidekickButton;
    public static CustomButton doomsayerButton;
    public static CustomButton eraserButton;
    public static CustomButton placeJackInTheBoxButton;
    public static CustomButton lightsOutButton;
    public static CustomButton cleanerCleanButton;
    public static CustomButton undertakerDragButton;
    public static CustomButton warlockCurseButton;
    public static CustomButton securityGuardButton;
    public static CustomButton securityGuardCamButton;
    public static CustomButton juggernautKillButton;
    public static CustomButton arsonistButton;
    public static CustomButton vultureEatButton;
    public static CustomButton yoyoButton;
    public static CustomButton yoyoAdminTableButton;
    public static CustomButton mediumButton;
    public static CustomButton pursuerButton;
    public static CustomButton witchSpellButton;
    public static CustomButton jumperButton;
    public static CustomButton escapistButton;
    public static CustomButton ninjaButton;
    public static CustomButton werewolfRampageButton;
    public static CustomButton werewolfKillButton;
    public static CustomButton minerMineButton;
    public static CustomButton mayorMeetingButton;
    public static CustomButton blackmailerButton;
    public static CustomButton thiefKillButton;
    public static CustomButton trapperButton;
    public static CustomButton terroristButton;
    public static CustomButton defuseButton;
    public static CustomButton zoomOutButton;
    private static CustomButton hunterLighterButton;
    private static CustomButton hunterAdminTableButton;
    private static CustomButton hunterArrowButton;
    private static CustomButton huntedShieldButton;
    public static CustomButton propDisguiseButton;
    private static CustomButton propHuntUnstuckButton;
    public static CustomButton propHuntRevealButton;
    private static CustomButton propHuntInvisButton;
    private static CustomButton propHuntSpeedboostButton;
    public static CustomButton propHuntAdminButton;
    public static CustomButton propHuntFindButton;

    public static Dictionary<byte, List<CustomButton>> deputyHandcuffedButtons;
    public static PoolablePlayer targetDisplay;
    public static GameObject propSpriteHolder;
    public static SpriteRenderer propSpriteRenderer;

    public static TMPro.TMP_Text securityGuardButtonScrewsText;
    public static TMPro.TMP_Text securityGuardChargesText;
    public static TMPro.TMP_Text deputyButtonHandcuffsText;
    public static TMPro.TMP_Text pursuerButtonBlanksText;
    public static TMPro.TMP_Text hackerAdminTableChargesText;
    public static TMPro.TMP_Text hackerVitalsChargesText;
    public static TMPro.TMP_Text trapperChargesText;
    public static TMPro.TMP_Text portalmakerButtonText1;
    public static TMPro.TMP_Text portalmakerButtonText2;
    public static TMPro.TMP_Text huntedShieldCountText;
    public static TMPro.TMP_Text disperserChargesText;

    public static void setCustomButtonCooldowns()
    {
        if (!initialized)
        {
            try
            {
                createButtonsPostfix(HudManager.Instance);
            }
            catch
            {
                Warn("Button cooldowns not set, either the gamemode does not require them or there's something wrong.");
                return;
            }
        }
        engineerRepairButton.MaxTimer = 0f;
        janitorCleanButton.MaxTimer = Janitor.cooldown;
        sheriffKillButton.MaxTimer = Sheriff.cooldown;
        deputyHandcuffButton.MaxTimer = Deputy.handcuffCooldown;
        timeMasterShieldButton.MaxTimer = TimeMaster.cooldown;
        veterenAlertButton.MaxTimer = Veteren.cooldown;
        medicShieldButton.MaxTimer = 0f;
        shifterShiftButton.MaxTimer = 0f;
        disperserDisperseButton.MaxTimer = 0f;
        morphlingButton.MaxTimer = Morphling.cooldown;
        bomberBombButton.MaxTimer = Bomber.cooldown;
        camouflagerButton.MaxTimer = Camouflager.cooldown;
        portalmakerPlacePortalButton.MaxTimer = Portalmaker.cooldown;
        usePortalButton.MaxTimer = Portalmaker.usePortalCooldown;
        portalmakerMoveToPortalButton.MaxTimer = Portalmaker.usePortalCooldown;
        hackerButton.MaxTimer = Hacker.cooldown;
        hackerVitalsButton.MaxTimer = Hacker.cooldown;
        hackerAdminTableButton.MaxTimer = Hacker.cooldown;
        vampireKillButton.MaxTimer = Vampire.cooldown;
        trackerTrackPlayerButton.MaxTimer = 0f;
        jumperButton.MaxTimer = Jumper.jumperJumpTime;
        disperserDisperseButton.MaxTimer = Disperser.cooldown;
        escapistButton.MaxTimer = Escapist.escapistEscapeTime;
        doomsayerButton.MaxTimer = Doomsayer.cooldown;
        bodyGuardGuardButton.MaxTimer = 0f;
        garlicButton.MaxTimer = 0f;
        yoyoButton.MaxTimer = Yoyo.markCooldown;
        yoyoAdminTableButton.MaxTimer = Yoyo.adminCooldown;
        yoyoAdminTableButton.EffectDuration = 10f;
        jackalKillButton.MaxTimer = Jackal.cooldown;
        werewolfKillButton.MaxTimer = Werewolf.killCooldown;
        sidekickKillButton.MaxTimer = Sidekick.cooldown;
        jackalSidekickButton.MaxTimer = Jackal.createSidekickCooldown;
        eraserButton.MaxTimer = Eraser.cooldown;
        placeJackInTheBoxButton.MaxTimer = Trickster.placeBoxCooldown;
        lightsOutButton.MaxTimer = Trickster.lightsOutCooldown;
        cleanerCleanButton.MaxTimer = Cleaner.cooldown;
        undertakerDragButton.MaxTimer = 0f;
        warlockCurseButton.MaxTimer = Warlock.cooldown;
        securityGuardButton.MaxTimer = SecurityGuard.cooldown;
        securityGuardCamButton.MaxTimer = SecurityGuard.cooldown;
        arsonistButton.MaxTimer = Arsonist.cooldown;
        vultureEatButton.MaxTimer = Vulture.cooldown;
        juggernautKillButton.MaxTimer = Juggernaut.cooldown;
        amnisiacRememberButton.MaxTimer = 0f;
        bomberGiveButton.MaxTimer = 0f;
        bomberGiveButton.Timer = 0f;
        mediumButton.MaxTimer = Medium.cooldown;
        pursuerButton.MaxTimer = Pursuer.cooldown;
        trackerTrackCorpsesButton.MaxTimer = Tracker.corpsesTrackingCooldown;
        witchSpellButton.MaxTimer = Witch.cooldown;
        ninjaButton.MaxTimer = Ninja.cooldown;
        swooperSwoopButton.MaxTimer = Jackal.swoopCooldown;
        minerMineButton.MaxTimer = Miner.cooldown;
        blackmailerButton.MaxTimer = Blackmailer.cooldown;
        thiefKillButton.MaxTimer = Thief.cooldown;
        mayorMeetingButton.MaxTimer = GameManager.Instance.LogicOptions.GetEmergencyCooldown();
        trapperButton.MaxTimer = Trapper.cooldown;
        terroristButton.MaxTimer = Terrorist.bombCooldown;
        hunterLighterButton.MaxTimer = Hunter.lightCooldown;
        hunterAdminTableButton.MaxTimer = Hunter.AdminCooldown;
        hunterArrowButton.MaxTimer = Hunter.ArrowCooldown;
        huntedShieldButton.MaxTimer = Hunted.shieldCooldown;
        defuseButton.MaxTimer = 0f;
        defuseButton.Timer = 0f;
        propDisguiseButton.MaxTimer = 1f;
        propHuntUnstuckButton.MaxTimer = PropHunt.unstuckCooldown;
        propHuntRevealButton.MaxTimer = PropHunt.revealCooldown;
        propHuntInvisButton.MaxTimer = PropHunt.invisCooldown;
        propHuntSpeedboostButton.MaxTimer = PropHunt.speedboostCooldown;
        propHuntAdminButton.MaxTimer = PropHunt.adminCooldown;
        propHuntFindButton.MaxTimer = PropHunt.findCooldown;

        timeMasterShieldButton.EffectDuration = TimeMaster.shieldDuration;
        veterenAlertButton.EffectDuration = Veteren.alertDuration;
        hackerButton.EffectDuration = Hacker.duration;
        hackerVitalsButton.EffectDuration = Hacker.duration;
        hackerAdminTableButton.EffectDuration = Hacker.duration;
        vampireKillButton.EffectDuration = Vampire.delay;
        swooperSwoopButton.EffectDuration = Jackal.duration;
        werewolfRampageButton.MaxTimer = Werewolf.rampageCooldown;
        werewolfRampageButton.EffectDuration = Werewolf.rampageDuration;
        minerMineButton.EffectDuration = Jackal.duration; // Jackal?
        camouflagerButton.EffectDuration = Camouflager.duration;
        morphlingButton.EffectDuration = Morphling.duration;
        bomberBombButton.EffectDuration = Bomber.bombDelay + Bomber.bombTimer;
        lightsOutButton.EffectDuration = Trickster.lightsOutDuration;
        arsonistButton.EffectDuration = Arsonist.duration;
        mediumButton.EffectDuration = Medium.duration;
        trackerTrackCorpsesButton.EffectDuration = Tracker.corpsesTrackingDuration;
        witchSpellButton.EffectDuration = Witch.spellCastingDuration;
        securityGuardCamButton.EffectDuration = SecurityGuard.duration;
        hunterLighterButton.EffectDuration = Hunter.lightDuration;
        hunterArrowButton.EffectDuration = Hunter.ArrowDuration;
        huntedShieldButton.EffectDuration = Hunted.shieldDuration;
        defuseButton.EffectDuration = Terrorist.defuseDuration;
        terroristButton.EffectDuration = Terrorist.destructionTime + Terrorist.bombActiveAfter;
        propHuntUnstuckButton.EffectDuration = PropHunt.unstuckDuration;
        propHuntRevealButton.EffectDuration = PropHunt.revealDuration;
        propHuntInvisButton.EffectDuration = PropHunt.invisDuration;
        propHuntSpeedboostButton.EffectDuration = PropHunt.speedboostDuration;
        propHuntAdminButton.EffectDuration = PropHunt.adminDuration;
        propHuntFindButton.EffectDuration = PropHunt.findDuration;
        // Already set the timer to the max, as the button is enabled during the game and not available at the start
        lightsOutButton.Timer = lightsOutButton.MaxTimer;
        zoomOutButton.MaxTimer = 0f;
        //changeChatButton.MaxTimer = 0f;
    }

    public static void showTargetNameOnButton(PlayerControl target, CustomButton button, string defaultText)
    {
        Helpers.showTargetNameOnButton(target, button, defaultText);
    }


    public static void showTargetNameOnButtonExplicit(PlayerControl target, CustomButton button, string defaultText)
    {
        Helpers.showTargetNameOnButtonExplicit(target, button, defaultText);
    }

    public static void resetTimeMasterButton()
    {
        timeMasterShieldButton.Timer = timeMasterShieldButton.MaxTimer;
        timeMasterShieldButton.isEffectActive = false;
        timeMasterShieldButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
        SoundEffectsManager.stop("timemasterShield");
    }

    public static void resetHuntedRewindButton()
    {
        huntedShieldButton.Timer = huntedShieldButton.MaxTimer;
        huntedShieldButton.isEffectActive = false;
        huntedShieldButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
        SoundEffectsManager.stop("timemasterShield");
    }

    private static void addReplacementHandcuffedButton(CustomButton button, Vector3? positionOffset = null, Func<bool> couldUse = null)
    {
        Vector3 positionOffsetValue = positionOffset ?? button.PositionOffset;  // For non custom buttons, we can set these manually.
        positionOffsetValue.z = -0.1f;
        couldUse = couldUse ?? button.CouldUse;
        CustomButton replacementHandcuffedButton = new(() => { }, () => { return true; }, couldUse, () => { }, Deputy.getHandcuffedButtonSprite(), positionOffsetValue, button.hudManager, button.hotkey,
            true, Deputy.handcuffDuration, () => { }, button.mirror);
        replacementHandcuffedButton.Timer = replacementHandcuffedButton.EffectDuration;
        replacementHandcuffedButton.actionButton.cooldownTimerText.color = new Color(0F, 0.8F, 0F);
        replacementHandcuffedButton.isEffectActive = true;
        if (deputyHandcuffedButtons.ContainsKey(CachedPlayer.LocalPlayer.PlayerId))
            deputyHandcuffedButtons[CachedPlayer.LocalPlayer.PlayerId].Add(replacementHandcuffedButton);
        else
            deputyHandcuffedButtons.Add(CachedPlayer.LocalPlayer.PlayerId, new List<CustomButton> { replacementHandcuffedButton });
    }

    // Disables / Enables all Buttons (except the ones disabled in the Deputy class), and replaces them with new buttons.
    public static void setAllButtonsHandcuffedStatus(bool handcuffed, bool reset = false)
    {
        if (reset)
        {
            deputyHandcuffedButtons = new Dictionary<byte, List<CustomButton>>();
            return;
        }
        if (handcuffed && !deputyHandcuffedButtons.ContainsKey(CachedPlayer.LocalPlayer.PlayerId))
        {
            int maxI = CustomButton.buttons.Count;
            for (int i = 0; i < maxI; i++)
            {
                try
                {
                    if (CustomButton.buttons[i].HasButton())  // For each custombutton the player has
                    {
                        addReplacementHandcuffedButton(CustomButton.buttons[i]);  // The new buttons are the only non-handcuffed buttons now!
                    }
                    CustomButton.buttons[i].isHandcuffed = true;
                }
                catch (NullReferenceException)
                {
                    System.Console.WriteLine("[WARNING] NullReferenceException from MeetingEndedUpdate().HasButton(), if theres only one warning its fine");  // Note: idk what this is good for, but i copied it from above /gendelo
                }
            }
            // Non Custom (Vanilla) Buttons. The Originals are disabled / hidden in UpdatePatch.cs already, just need to replace them. Can use any button, as we replace onclick etc anyways.
            // Kill Button if enabled for the Role
            if (FastDestroyableSingleton<HudManager>.Instance.KillButton.isActiveAndEnabled) addReplacementHandcuffedButton(arsonistButton, CustomButton.ButtonPositions.upperRowRight, couldUse: () => { return FastDestroyableSingleton<HudManager>.Instance.KillButton.currentTarget != null; });
            // Vent Button if enabled
            if (CachedPlayer.LocalPlayer.PlayerControl.roleCanUseVents()) addReplacementHandcuffedButton(arsonistButton, CustomButton.ButtonPositions.upperRowCenter, couldUse: () => { return FastDestroyableSingleton<HudManager>.Instance.ImpostorVentButton.currentTarget != null; });
            // Report Button
            addReplacementHandcuffedButton(arsonistButton, (!CachedPlayer.LocalPlayer.Data.Role.IsImpostor) ? new Vector3(-1f, -0.06f, 0) : CustomButton.ButtonPositions.lowerRowRight, () => { return FastDestroyableSingleton<HudManager>.Instance.ReportButton.graphic.color == Palette.EnabledColor; });
        }
        else if (!handcuffed && deputyHandcuffedButtons.ContainsKey(CachedPlayer.LocalPlayer.PlayerId))  // Reset to original. Disables the replacements, enables the original buttons.
        {
            foreach (CustomButton replacementButton in deputyHandcuffedButtons[CachedPlayer.LocalPlayer.PlayerId])
            {
                replacementButton.HasButton = () => { return false; };
                replacementButton.Update(); // To make it disappear properly.
                CustomButton.buttons.Remove(replacementButton);
            }
            deputyHandcuffedButtons.Remove(CachedPlayer.LocalPlayer.PlayerId);

            foreach (CustomButton button in CustomButton.buttons)
            {
                button.isHandcuffed = false;
            }
        }
    }

    private static void setButtonTargetDisplay(PlayerControl target, CustomButton button = null, Vector3? offset = null)
    {
        if (target == null || button == null)
        {
            if (targetDisplay != null)
            {  // Reset the poolable player
                targetDisplay.gameObject.SetActive(false);
                UnityEngine.Object.Destroy(targetDisplay.gameObject);
                targetDisplay = null;
            }
            return;
        }
        // Add poolable player to the button so that the target outfit is shown
        button.actionButton.cooldownTimerText.transform.localPosition = new Vector3(0, 0, -1f);  // Before the poolable player
        targetDisplay = UnityEngine.Object.Instantiate(IntroCutsceneOnDestroyPatch.playerPrefab, button.actionButton.transform);
        NetworkedPlayerInfo data = target.Data;
        target.SetPlayerMaterialColors(targetDisplay.cosmetics.currentBodySprite.BodySprite);
        targetDisplay.SetSkin(data.DefaultOutfit.SkinId, data.DefaultOutfit.ColorId);
        targetDisplay.SetHat(data.DefaultOutfit.HatId, data.DefaultOutfit.ColorId);
        targetDisplay.cosmetics.nameText.text = "";  // Hide the name!
        targetDisplay.transform.localPosition = new Vector3(0f, 0.22f, -0.01f);
        if (offset != null) targetDisplay.transform.localPosition += (Vector3)offset;
        targetDisplay.transform.localScale = Vector3.one * 0.33f;
        targetDisplay.setSemiTransparent(false);
        targetDisplay.gameObject.SetActive(true);
    }

    public static void Postfix(HudManager __instance)
    {
        initialized = false;

        try
        {
            createButtonsPostfix(__instance);
        }
        catch { }
    }

    public static void createButtonsPostfix(HudManager __instance)
    {
        // get map id, or raise error to wait...
        var mapId = GameOptionsManager.Instance.currentNormalGameOptions.MapId;

        // Engineer Repair
        engineerRepairButton = new CustomButton(
            () =>
            {
                engineerRepairButton.Timer = 0f;
                MessageWriter usedRepairWriter = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.EngineerUsedRepair, SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(usedRepairWriter);
                RPCProcedure.engineerUsedRepair();
                SoundEffectsManager.play("engineerRepair");
                foreach (PlayerTask task in CachedPlayer.LocalPlayer.PlayerControl.myTasks.GetFastEnumerator())
                {
                    if (task.TaskType == TaskTypes.FixLights)
                    {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.EngineerFixLights, SendOption.Reliable, -1);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.engineerFixLights();
                    }
                    else if (task.TaskType == TaskTypes.RestoreOxy)
                    {
                        MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.LifeSupp, 0 | 64);
                        MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.LifeSupp, 1 | 64);
                    }
                    else if (task.TaskType == TaskTypes.ResetReactor)
                    {
                        MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.Reactor, 16);
                    }
                    else if (task.TaskType == TaskTypes.ResetSeismic)
                    {
                        MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.Laboratory, 16);
                    }
                    else if (task.TaskType == TaskTypes.FixComms)
                    {
                        MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.Comms, 16 | 0);
                        MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.Comms, 16 | 1);
                    }
                    else if (task.TaskType == TaskTypes.StopCharles)
                    {
                        MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.Reactor, 0 | 16);
                        MapUtilities.CachedShipStatus.RpcRepairSystem(SystemTypes.Reactor, 1 | 16);
                    }
                    else if (SubmergedCompatibility.IsSubmerged && task.TaskType == SubmergedCompatibility.RetrieveOxygenMask)
                    {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.EngineerFixSubmergedOxygen, SendOption.Reliable, -1);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.engineerFixSubmergedOxygen();
                    }

                }
            },
            () => { return Engineer.engineer != null && Engineer.engineer == CachedPlayer.LocalPlayer.PlayerControl && Engineer.remainingFixes > 0 && Engineer.remoteFix && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () =>
            {
                bool sabotageActive = false;
                foreach (PlayerTask task in CachedPlayer.LocalPlayer.PlayerControl.myTasks.GetFastEnumerator())
                    if (task.TaskType == TaskTypes.FixLights || task.TaskType == TaskTypes.RestoreOxy || task.TaskType == TaskTypes.ResetReactor || task.TaskType == TaskTypes.ResetSeismic || task.TaskType == TaskTypes.FixComms || task.TaskType == TaskTypes.StopCharles
                        || (SubmergedCompatibility.IsSubmerged && task.TaskType == SubmergedCompatibility.RetrieveOxygenMask))
                        sabotageActive = true;
                return sabotageActive && Engineer.remainingFixes > 0 && CachedPlayer.LocalPlayer.PlayerControl.CanMove && !Engineer.usedFix;
            },
            () => { if (Engineer.resetFixAfterMeeting) Engineer.resetFixes(); },
            Engineer.getButtonSprite(),
            CustomButton.ButtonPositions.upperRowRight,
            __instance,
            KeyCode.F
        );

        // Janitor Clean
        janitorCleanButton = new CustomButton(
            () =>
            {
                foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(CachedPlayer.LocalPlayer.PlayerControl.GetTruePosition(), CachedPlayer.LocalPlayer.PlayerControl.MaxReportDistance, Constants.PlayersOnlyMask))
                {
                    if (collider2D.tag == "DeadBody")
                    {
                        DeadBody component = collider2D.GetComponent<DeadBody>();
                        if (component && !component.Reported)
                        {
                            Vector2 truePosition = CachedPlayer.LocalPlayer.PlayerControl.GetTruePosition();
                            Vector2 truePosition2 = component.TruePosition;
                            if (Vector2.Distance(truePosition2, truePosition) <= CachedPlayer.LocalPlayer.PlayerControl.MaxReportDistance && CachedPlayer.LocalPlayer.PlayerControl.CanMove && !PhysicsHelpers.AnythingBetween(truePosition, truePosition2, Constants.ShipAndObjectsMask, false))
                            {
                                NetworkedPlayerInfo playerInfo = GameData.Instance.GetPlayerById(component.ParentId);

                                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.CleanBody, SendOption.Reliable, -1);
                                writer.Write(playerInfo.PlayerId);
                                writer.Write(Janitor.janitor.PlayerId);
                                AmongUsClient.Instance.FinishRpcImmediately(writer);
                                RPCProcedure.cleanBody(playerInfo.PlayerId, Janitor.janitor.PlayerId);
                                janitorCleanButton.Timer = janitorCleanButton.MaxTimer;
                                SoundEffectsManager.play("cleanerClean");

                                break;
                            }
                        }
                    }
                }
            },
            () => { return Janitor.janitor != null && Janitor.janitor == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () => { return __instance.ReportButton.graphic.color == Palette.EnabledColor && CachedPlayer.LocalPlayer.PlayerControl.CanMove; },
            () => { janitorCleanButton.Timer = janitorCleanButton.MaxTimer; },
            Janitor.getButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.F
        );
        /*
                    // Sheriff Kill
                    sheriffKillButton = new CustomButton(
                        () => {
                            if (Helpers.checkAndDoVetKill(Sheriff.currentTarget)) return;
                            MurderAttemptResult murderAttemptResult = Helpers.checkMuderAttempt(Sheriff.sheriff, Sheriff.currentTarget);
                            if (murderAttemptResult == MurderAttemptResult.SuppressKill) return;

                            if (murderAttemptResult == MurderAttemptResult.PerformKill) {
                                byte targetId = 0;
                                if ((Sheriff.currentTarget.Data.Role.IsImpostor && (Sheriff.currentTarget != Mini.mini || Mini.isGrownUp())) ||
                                    (Sheriff.spyCanDieToSheriff && Spy.spy == Sheriff.currentTarget) ||
                                    (Sheriff.canKillNeutrals && Helpers.isNeutral(Sheriff.currentTarget)) ||
                                    (Jackal.jackal == Sheriff.currentTarget || Sidekick.sidekick == Sheriff.currentTarget)) {
                                    targetId = Sheriff.currentTarget.PlayerId;
                                }
                                else {
                                    targetId = CachedPlayer.LocalPlayer.PlayerId;
                                }

                                MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.UncheckedMurderPlayer, Hazel.SendOption.Reliable, -1);
                                killWriter.Write(Sheriff.sheriff.Data.PlayerId);
                                killWriter.Write(targetId);
                                killWriter.Write(byte.MaxValue);
                                AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                                RPCProcedure.uncheckedMurderPlayer(Sheriff.sheriff.Data.PlayerId, targetId, Byte.MaxValue);
                            }
                             if (murderAttemptResult == MurderAttemptResult.BodyGuardKill) {
                                Helpers.checkMuderAttemptAndKill(Sheriff.sheriff, Sheriff.currentTarget);
                            }

                            sheriffKillButton.Timer = sheriffKillButton.MaxTimer;
                            Sheriff.currentTarget = null;
                        },
                        () => { return Sheriff.sheriff != null && Sheriff.sheriff == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
                        () => { 
                            showTargetNameOnButton(Sheriff.currentTarget, sheriffKillButton, "KILL");
                            return Sheriff.currentTarget && CachedPlayer.LocalPlayer.PlayerControl.CanMove; 
                            },
                        () => { sheriffKillButton.Timer = sheriffKillButton.MaxTimer;},
                        __instance.KillButton.graphic.sprite,
                        CustomButton.ButtonPositions.upperRowRight,
                        __instance,
                        KeyCode.Q
                    );
                    */
        //Sheriff Kill
        sheriffKillButton = new CustomButton(
            () =>
            {
                if (checkAndDoVetKill(Sheriff.currentTarget)) return;
                MurderAttemptResult murderAttemptResult = checkMuderAttempt(Sheriff.sheriff, Sheriff.currentTarget);
                if (murderAttemptResult == MurderAttemptResult.SuppressKill) return;
                if (murderAttemptResult == MurderAttemptResult.PerformKill)
                {
                    byte targetId = 0;
                    if ((Sheriff.currentTarget != Mini.mini || Mini.isGrownUp()) &&
                    (Sheriff.currentTarget.Data.Role.IsImpostor ||
                    Jackal.jackal == Sheriff.currentTarget ||
                    Juggernaut.juggernaut == Sheriff.currentTarget ||
                    Sidekick.sidekick == Sheriff.currentTarget ||
                    Werewolf.werewolf == Sheriff.currentTarget ||
                        (Sheriff.spyCanDieToSheriff && Spy.spy == Sheriff.currentTarget) ||
                        (Sheriff.canKillNeutrals &&
                        ((Arsonist.arsonist == Sheriff.currentTarget && Sheriff.canKillArsonist) ||
                        (Jester.jester == Sheriff.currentTarget && Sheriff.canKillJester) ||
                        (Vulture.vulture == Sheriff.currentTarget && Sheriff.canKillVulture) ||
                        (Lawyer.lawyer == Sheriff.currentTarget && Sheriff.canKillLawyer && !Lawyer.isProsecutor) ||
                        (Thief.thief == Sheriff.currentTarget && Sheriff.canKillThief) ||
                        (Amnisiac.amnisiac == Sheriff.currentTarget && Sheriff.canKillAmnesiac) ||
                        (Doomsayer.doomsayer == Sheriff.currentTarget && Sheriff.canKillDoomsayer) ||
                        (Lawyer.lawyer == Sheriff.currentTarget && Sheriff.canKillProsecutor && Lawyer.isProsecutor) ||
                        (Pursuer.pursuer == Sheriff.currentTarget && Sheriff.canKillPursuer)))))
                    {
                        targetId = Sheriff.currentTarget.PlayerId;
                    }
                    else if (Sheriff.misfireKills == 0)
                    {
                        targetId = CachedPlayer.LocalPlayer.PlayerId;
                    }
                    else if (Sheriff.misfireKills == 1)
                    {
                        targetId = Sheriff.currentTarget.PlayerId;
                    }
                    else if (Sheriff.misfireKills == 2)
                    {
                        targetId = Sheriff.currentTarget.PlayerId;
                        MessageWriter killWriter2 = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.UncheckedMurderPlayer, SendOption.Reliable, -1);
                        killWriter2.Write(Sheriff.sheriff.Data.PlayerId);
                        killWriter2.Write(CachedPlayer.LocalPlayer.PlayerId);
                        killWriter2.Write(byte.MaxValue);
                        AmongUsClient.Instance.FinishRpcImmediately(killWriter2);
                        RPCProcedure.uncheckedMurderPlayer(Sheriff.sheriff.Data.PlayerId, CachedPlayer.LocalPlayer.PlayerId, Byte.MaxValue);
                    }

                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.UncheckedMurderPlayer, SendOption.Reliable, -1);
                    killWriter.Write(Sheriff.sheriff.Data.PlayerId);
                    killWriter.Write(targetId);
                    killWriter.Write(byte.MaxValue);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.uncheckedMurderPlayer(Sheriff.sheriff.Data.PlayerId, targetId, Byte.MaxValue);
                }
                if (murderAttemptResult == MurderAttemptResult.BodyGuardKill)
                {
                    checkMuderAttemptAndKill(Sheriff.sheriff, Sheriff.currentTarget);
                }

                sheriffKillButton.Timer = sheriffKillButton.MaxTimer;
                Sheriff.currentTarget = null;
            },
            () => { return Sheriff.sheriff != null && Sheriff.sheriff == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () =>
            {
                showTargetNameOnButton(Sheriff.currentTarget, sheriffKillButton, "KILL");
                return Sheriff.currentTarget && CachedPlayer.LocalPlayer.PlayerControl.CanMove;
            },
            () => { sheriffKillButton.Timer = sheriffKillButton.MaxTimer; },
            __instance.KillButton.graphic.sprite,
            CustomButton.ButtonPositions.upperRowRight,
            __instance,
            KeyCode.Q
        );

        // Deputy Handcuff
        deputyHandcuffButton = new CustomButton(
            () =>
            {
                byte targetId = 0;
                PlayerControl target = Sheriff.sheriff == CachedPlayer.LocalPlayer.PlayerControl ? Sheriff.currentTarget : Deputy.currentTarget;  // If the deputy is now the sheriff, sheriffs target, else deputies target
                checkWatchFlash(target);
                targetId = target.PlayerId;
                if (checkAndDoVetKill(target)) return;
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.DeputyUsedHandcuffs, SendOption.Reliable, -1);
                writer.Write(targetId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.deputyUsedHandcuffs(targetId);
                Deputy.currentTarget = null;
                deputyHandcuffButton.Timer = deputyHandcuffButton.MaxTimer;

                SoundEffectsManager.play("deputyHandcuff");
            },
            () => { return ((Deputy.deputy != null && Deputy.deputy == CachedPlayer.LocalPlayer.PlayerControl) || (Sheriff.sheriff != null && Sheriff.sheriff == CachedPlayer.LocalPlayer.PlayerControl && Sheriff.sheriff == Sheriff.formerDeputy && Deputy.keepsHandcuffsOnPromotion)) && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () =>
            {
                showTargetNameOnButton(Deputy.currentTarget, deputyHandcuffButton, "CUFF");
                if (deputyButtonHandcuffsText != null) deputyButtonHandcuffsText.text = $"{Deputy.remainingHandcuffs}";
                return ((Deputy.deputy != null && Deputy.deputy == CachedPlayer.LocalPlayer.PlayerControl && Deputy.currentTarget) || (Sheriff.sheriff != null && Sheriff.sheriff == CachedPlayer.LocalPlayer.PlayerControl && Sheriff.sheriff == Sheriff.formerDeputy && Sheriff.currentTarget)) && Deputy.remainingHandcuffs > 0 && CachedPlayer.LocalPlayer.PlayerControl.CanMove;
            },
            () => { deputyHandcuffButton.Timer = deputyHandcuffButton.MaxTimer; },
            Deputy.getButtonSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F
        );
        // Deputy Handcuff button handcuff counter
        deputyButtonHandcuffsText = UnityEngine.Object.Instantiate(deputyHandcuffButton.actionButton.cooldownTimerText, deputyHandcuffButton.actionButton.cooldownTimerText.transform.parent);
        deputyButtonHandcuffsText.text = "";
        deputyButtonHandcuffsText.enableWordWrapping = false;
        deputyButtonHandcuffsText.transform.localScale = Vector3.one * 0.5f;
        deputyButtonHandcuffsText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

        // Time Master Rewind Time
        timeMasterShieldButton = new CustomButton(
            () =>
            {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.TimeMasterShield, SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.timeMasterShield();
                SoundEffectsManager.play("timemasterShield");
            },
            () => { return TimeMaster.timeMaster != null && TimeMaster.timeMaster == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () => { return CachedPlayer.LocalPlayer.PlayerControl.CanMove; },
            () =>
            {
                timeMasterShieldButton.Timer = timeMasterShieldButton.MaxTimer;
                timeMasterShieldButton.isEffectActive = false;
                timeMasterShieldButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            },
            TimeMaster.getButtonSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F,
            true,
            TimeMaster.shieldDuration,
            () =>
            {
                timeMasterShieldButton.Timer = timeMasterShieldButton.MaxTimer;
                SoundEffectsManager.stop("timemasterShield");

            }
        );

        // Veteren Alert
        veterenAlertButton = new CustomButton(
            () =>
            {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.VeterenAlert, SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.veterenAlert();
            },
            () => { return Veteren.veteren != null && Veteren.veteren == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () => { return CachedPlayer.LocalPlayer.PlayerControl.CanMove; },
            () =>
            {
                veterenAlertButton.Timer = veterenAlertButton.MaxTimer;
                veterenAlertButton.isEffectActive = false;
                veterenAlertButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            },
            Veteren.getButtonSprite(),
            CustomButton.ButtonPositions.lowerRowRight, //brb
            __instance,
            KeyCode.F,
            true,
            Veteren.alertDuration,
            () => { veterenAlertButton.Timer = veterenAlertButton.MaxTimer; }
        );

        // Medic Shield
        medicShieldButton = new CustomButton(
            () =>
            {
                if (checkAndDoVetKill(Medic.currentTarget)) return;
                checkWatchFlash(Medic.currentTarget);

                medicShieldButton.Timer = 0f;

                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, Medic.setShieldAfterMeeting ? (byte)CustomRPC.SetFutureShielded : (byte)CustomRPC.MedicSetShielded, SendOption.Reliable, -1);
                writer.Write(Medic.currentTarget.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                if (Medic.setShieldAfterMeeting)
                    RPCProcedure.setFutureShielded(Medic.currentTarget.PlayerId);
                else
                    RPCProcedure.medicSetShielded(Medic.currentTarget.PlayerId);
                Medic.meetingAfterShielding = false;

                SoundEffectsManager.play("medicShield");
            },
            () => { return Medic.medic != null && Medic.medic == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () =>
            {
                showTargetNameOnButton(Medic.currentTarget, medicShieldButton, "SHIELD");
                return !Medic.usedShield && Medic.currentTarget && CachedPlayer.LocalPlayer.PlayerControl.CanMove;
            },
            () => { if (Medic.reset) Medic.resetShielded(); },
            Medic.getButtonSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F
        );


        // Shifter shift
        shifterShiftButton = new CustomButton(
            () =>
            {
                if (checkAndDoVetKill(Shifter.currentTarget)) return;
                checkWatchFlash(Shifter.currentTarget);

                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.SetFutureShifted, SendOption.Reliable, -1);
                writer.Write(Shifter.currentTarget.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.setFutureShifted(Shifter.currentTarget.PlayerId);
                SoundEffectsManager.play("shifterShift");
            },
            () => { return Shifter.shifter != null && Shifter.shifter == CachedPlayer.LocalPlayer.PlayerControl && Shifter.futureShift == null && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () =>
            {
                showTargetNameOnButton(Shifter.currentTarget, shifterShiftButton, "SHIFT");
                return Shifter.currentTarget && Shifter.futureShift == null && CachedPlayer.LocalPlayer.PlayerControl.CanMove;
            },
            () => { },
            Shifter.getButtonSprite(),
            new Vector3(0, 1f, 0),
            __instance,
            null,
            true
        );

        // Disperser disperse
        disperserDisperseButton = new CustomButton(
            () =>
            {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.Disperse, SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.disperse();
                SoundEffectsManager.play("disperserDisperse");

                disperserDisperseButton.Timer = disperserDisperseButton.MaxTimer;
            },
            () => { return Disperser.disperser != null && Disperser.disperser == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () =>
            {
                if (disperserChargesText != null) disperserChargesText.text = $"{Disperser.remainingDisperses}";
                return Disperser.remainingDisperses > 0 && CachedPlayer.LocalPlayer.PlayerControl.CanMove;
            },
            () =>
            {
                if (Disperser.remainingDisperses > 0) disperserDisperseButton.Timer = disperserDisperseButton.MaxTimer;
            },
            Disperser.getButtonSprite(),
            new Vector3(0, 1f, 0),
            __instance,
            null,
            true,
            buttonText: "DISPERSE"
        );
        disperserChargesText = UnityEngine.Object.Instantiate(disperserDisperseButton.actionButton.cooldownTimerText, disperserDisperseButton.actionButton.cooldownTimerText.transform.parent);
        disperserChargesText.text = "";
        disperserChargesText.enableWordWrapping = false;
        disperserChargesText.transform.localScale = Vector3.one * 0.5f;
        disperserChargesText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

        // Morphling morph

        morphlingButton = new CustomButton(
            () =>
            {
                if (Morphling.sampledTarget != null)
                {
                    if (checkAndDoVetKill(Morphling.currentTarget)) return;
                    checkWatchFlash(Morphling.currentTarget);
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.MorphlingMorph, SendOption.Reliable, -1);
                    writer.Write(Morphling.sampledTarget.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.morphlingMorph(Morphling.sampledTarget.PlayerId);
                    Morphling.sampledTarget = null;
                    morphlingButton.EffectDuration = Morphling.duration;
                    SoundEffectsManager.play("morphlingMorph");
                }
                else if (Morphling.currentTarget != null)
                {
                    Morphling.sampledTarget = Morphling.currentTarget;
                    morphlingButton.Sprite = Morphling.getMorphSprite();
                    morphlingButton.EffectDuration = 1f;
                    SoundEffectsManager.play("morphlingSample");

                    // Add poolable player to the button so that the target outfit is shown
                    setButtonTargetDisplay(Morphling.sampledTarget, morphlingButton);
                }
            },
            () => { return Morphling.morphling != null && Morphling.morphling == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () =>
            {
                if (Morphling.sampledTarget == null)
                    showTargetNameOnButton(Morphling.currentTarget, morphlingButton, "SAMPLE");
                return (Morphling.currentTarget || Morphling.sampledTarget) && !isActiveCamoComms() && CachedPlayer.LocalPlayer.PlayerControl.CanMove && !MushroomSabotageActive();
            },
            () =>
            {
                morphlingButton.Timer = morphlingButton.MaxTimer;
                morphlingButton.Sprite = Morphling.getSampleSprite();
                morphlingButton.isEffectActive = false;
                morphlingButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                Morphling.sampledTarget = null;
                setButtonTargetDisplay(null);
            },
            Morphling.getSampleSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.F,
            true,
            Morphling.duration,
            () =>
            {
                if (Morphling.sampledTarget == null)
                {
                    morphlingButton.Timer = morphlingButton.MaxTimer;
                    morphlingButton.Sprite = Morphling.getSampleSprite();
                    SoundEffectsManager.play("morphlingMorph");

                    // Reset the poolable player
                    setButtonTargetDisplay(null);
                }
            }
        );

        // Camouflager camouflage
        camouflagerButton = new CustomButton(
            () =>
            {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.CamouflagerCamouflage, SendOption.Reliable, -1);
                writer.Write(1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.camouflagerCamouflage(1);
                SoundEffectsManager.play("morphlingMorph");

            },
            () => { return Camouflager.camouflager != null && Camouflager.camouflager == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () => { return !isActiveCamoComms() && CachedPlayer.LocalPlayer.PlayerControl.CanMove; },
            () =>
            {
                camouflagerButton.Timer = camouflagerButton.MaxTimer;
                camouflagerButton.isEffectActive = false;
                camouflagerButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            },
            Camouflager.getButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.F,
            true,
            Camouflager.duration,
            () =>
            {
                camouflagerButton.Timer = camouflagerButton.MaxTimer;
                SoundEffectsManager.play("morphlingMorph");
            }
        );

        // Hacker button
        hackerButton = new CustomButton(
            () =>
            {
                Hacker.hackerTimer = Hacker.duration;
                SoundEffectsManager.play("hackerHack");
            },
            () => { return Hacker.hacker != null && Hacker.hacker == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () => { return true; },
            () =>
            {
                hackerButton.Timer = hackerButton.MaxTimer;
                hackerButton.isEffectActive = false;
                hackerButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            },
            Hacker.getButtonSprite(),
            CustomButton.ButtonPositions.upperRowRight,
            __instance,
            KeyCode.F,
            true,
            0f,
            () => { hackerButton.Timer = hackerButton.MaxTimer; }
        );

        hackerAdminTableButton = new CustomButton(
           () =>
           {
               if (!MapBehaviour.Instance || !MapBehaviour.Instance.isActiveAndEnabled)
               {
                   HudManager __instance = FastDestroyableSingleton<HudManager>.Instance;
                   __instance.InitMap();
                   MapBehaviour.Instance.ShowCountOverlay(allowedToMove: true, showLivePlayerPosition: true, includeDeadBodies: true);
               }
               if (Hacker.cantMove) CachedPlayer.LocalPlayer.PlayerControl.moveable = false;
               CachedPlayer.LocalPlayer.NetTransform.Halt(); // Stop current movement 
               Hacker.chargesAdminTable--;
           },
           () => { return Hacker.hacker != null && Hacker.hacker == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
           () =>
           {
               if (hackerAdminTableChargesText != null) hackerAdminTableChargesText.text = $"{Hacker.chargesAdminTable} / {Hacker.toolsNumber}";
               return Hacker.chargesAdminTable > 0;
           },
           () =>
           {
               hackerAdminTableButton.Timer = hackerAdminTableButton.MaxTimer;
               hackerAdminTableButton.isEffectActive = false;
               hackerAdminTableButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
           },
           Hacker.getAdminSprite(),
           CustomButton.ButtonPositions.lowerRowRight,
           __instance,
           KeyCode.Q,
           true,
           0f,
           () =>
           {
               hackerAdminTableButton.Timer = hackerAdminTableButton.MaxTimer;
               if (!hackerVitalsButton.isEffectActive) CachedPlayer.LocalPlayer.PlayerControl.moveable = true;
               if (MapBehaviour.Instance && MapBehaviour.Instance.isActiveAndEnabled) MapBehaviour.Instance.Close();
           },
           GameOptionsManager.Instance.currentNormalGameOptions.MapId == 3,
           "ADMIN"
       );

        // Hacker Admin Table Charges
        hackerAdminTableChargesText = UnityEngine.Object.Instantiate(hackerAdminTableButton.actionButton.cooldownTimerText, hackerAdminTableButton.actionButton.cooldownTimerText.transform.parent);
        hackerAdminTableChargesText.text = "";
        hackerAdminTableChargesText.enableWordWrapping = false;
        hackerAdminTableChargesText.transform.localScale = Vector3.one * 0.5f;
        hackerAdminTableChargesText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

        hackerVitalsButton = new CustomButton(
           () =>
           {
               if (GameOptionsManager.Instance.currentNormalGameOptions.MapId != 1)
               {
                   if (Hacker.vitals == null)
                   {
                       var e = UnityEngine.Object.FindObjectsOfType<SystemConsole>().FirstOrDefault(x => x.gameObject.name.Contains("panel_vitals") || x.gameObject.name.Contains("Vitals"));
                       if (e == null || Camera.main == null) return;
                       Hacker.vitals = UnityEngine.Object.Instantiate(e.MinigamePrefab, Camera.main.transform, false);
                   }
                   Hacker.vitals.transform.SetParent(Camera.main.transform, false);
                   Hacker.vitals.transform.localPosition = new Vector3(0.0f, 0.0f, -50f);
                   Hacker.vitals.Begin(null);
               }
               else
               {
                   if (Hacker.doorLog == null)
                   {
                       var e = UnityEngine.Object.FindObjectsOfType<SystemConsole>().FirstOrDefault(x => x.gameObject.name.Contains("SurvLogConsole"));
                       if (e == null || Camera.main == null) return;
                       Hacker.doorLog = UnityEngine.Object.Instantiate(e.MinigamePrefab, Camera.main.transform, false);
                   }
                   Hacker.doorLog.transform.SetParent(Camera.main.transform, false);
                   Hacker.doorLog.transform.localPosition = new Vector3(0.0f, 0.0f, -50f);
                   Hacker.doorLog.Begin(null);
               }

               if (Hacker.cantMove) CachedPlayer.LocalPlayer.PlayerControl.moveable = false;
               CachedPlayer.LocalPlayer.NetTransform.Halt(); // Stop current movement 

               Hacker.chargesVitals--;
           },
           () => { return Hacker.hacker != null && Hacker.hacker == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead && GameOptionsManager.Instance.currentGameOptions.MapId != 0 && GameOptionsManager.Instance.currentNormalGameOptions.MapId != 3; },
           () =>
           {
               if (hackerVitalsChargesText != null) hackerVitalsChargesText.text = $"{Hacker.chargesVitals} / {Hacker.toolsNumber}";
               hackerVitalsButton.actionButton.graphic.sprite = IsMira ? Hacker.getLogSprite() : Hacker.getVitalsSprite();
               hackerVitalsButton.actionButton.OverrideText(IsMira ? "DOORLOG" : "VITALS");
               return Hacker.chargesVitals > 0;
           },
           () =>
           {
               hackerVitalsButton.Timer = hackerVitalsButton.MaxTimer;
               hackerVitalsButton.isEffectActive = false;
               hackerVitalsButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
           },
           Hacker.getVitalsSprite(),
           CustomButton.ButtonPositions.lowerRowCenter,
           __instance,
           KeyCode.Q,
           true,
           0f,
           () =>
           {
               hackerVitalsButton.Timer = hackerVitalsButton.MaxTimer;
               if (!hackerAdminTableButton.isEffectActive) CachedPlayer.LocalPlayer.PlayerControl.moveable = true;
               if (Minigame.Instance)
               {
                   if (IsMira) Hacker.doorLog.ForceClose();
                   else Hacker.vitals.ForceClose();
               }
           },
           false,
          IsMira ? "DOORLOG" : "VITALS"
       );

        // Hacker Vitals Charges
        hackerVitalsChargesText = UnityEngine.Object.Instantiate(hackerVitalsButton.actionButton.cooldownTimerText, hackerVitalsButton.actionButton.cooldownTimerText.transform.parent);
        hackerVitalsChargesText.text = "";
        hackerVitalsChargesText.enableWordWrapping = false;
        hackerVitalsChargesText.transform.localScale = Vector3.one * 0.5f;
        hackerVitalsChargesText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

        // Tracker button
        trackerTrackPlayerButton = new CustomButton(
            () =>
            {
                if (checkAndDoVetKill(Tracker.currentTarget)) return;
                checkWatchFlash(Tracker.currentTarget);

                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.TrackerUsedTracker, SendOption.Reliable, -1);
                writer.Write(Tracker.currentTarget.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.trackerUsedTracker(Tracker.currentTarget.PlayerId);
                SoundEffectsManager.play("trackerTrackPlayer");
            },
            () => { return Tracker.tracker != null && Tracker.tracker == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () =>
            {
                if (!Tracker.usedTracker)
                    showTargetNameOnButton(Tracker.currentTarget, trackerTrackPlayerButton, "");
                return CachedPlayer.LocalPlayer.PlayerControl.CanMove && Tracker.currentTarget != null && !Tracker.usedTracker;
            },
            () => { if (Tracker.resetTargetAfterMeeting) Tracker.resetTracked(); },
            Tracker.getButtonSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F
        );

        trackerTrackCorpsesButton = new CustomButton(
            () =>
            {
                Tracker.corpsesTrackingTimer = Tracker.corpsesTrackingDuration;
                SoundEffectsManager.play("trackerTrackCorpses");
            },
            () => { return Tracker.tracker != null && Tracker.tracker == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead && Tracker.canTrackCorpses; },
            () => { return CachedPlayer.LocalPlayer.PlayerControl.CanMove; },
            () =>
            {
                trackerTrackCorpsesButton.Timer = trackerTrackCorpsesButton.MaxTimer;
                trackerTrackCorpsesButton.isEffectActive = false;
                trackerTrackCorpsesButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            },
            Tracker.getTrackCorpsesButtonSprite(),
            CustomButton.ButtonPositions.lowerRowCenter,
            __instance,
            KeyCode.Q,
            true,
            Tracker.corpsesTrackingDuration,
            () =>
            {
                trackerTrackCorpsesButton.Timer = trackerTrackCorpsesButton.MaxTimer;
            }
        );

        bodyGuardGuardButton = new CustomButton(
            () =>
            {
                if (checkAndDoVetKill(BodyGuard.currentTarget)) return;
                checkWatchFlash(BodyGuard.currentTarget);
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.BodyGuardGuardPlayer, SendOption.Reliable, -1);
                writer.Write(BodyGuard.currentTarget.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.bodyGuardGuardPlayer(BodyGuard.currentTarget.PlayerId);
                // SoundEffectsManager.play("trackerTrackPlayer");
            },
            () => { return BodyGuard.bodyguard != null && BodyGuard.bodyguard == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () =>
            {
                if (!BodyGuard.usedGuard)
                    showTargetNameOnButton(BodyGuard.currentTarget, bodyGuardGuardButton, "Guard");
                return CachedPlayer.LocalPlayer.PlayerControl.CanMove && BodyGuard.currentTarget != null && !BodyGuard.usedGuard;
            },
            () => { if (BodyGuard.reset) BodyGuard.resetGuarded(); },
            BodyGuard.getGuardButtonSprite(),
            CustomButton.ButtonPositions.lowerRowRight, //brb
            __instance,
            KeyCode.F
        );

        privateInvestigatorWatchButton = new CustomButton(
            () =>
            {
                if (checkAndDoVetKill(PrivateInvestigator.currentTarget)) return;
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.PrivateInvestigatorWatchPlayer, SendOption.Reliable, -1);
                writer.Write(PrivateInvestigator.currentTarget.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.privateInvestigatorWatchPlayer(PrivateInvestigator.currentTarget.PlayerId);
                // SoundEffectsManager.play("trackerTrackPlayer");
            },
            () => { return PrivateInvestigator.privateInvestigator != null && PrivateInvestigator.privateInvestigator == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () =>
            {
                if (PrivateInvestigator.watching == null)
                    showTargetNameOnButton(PrivateInvestigator.currentTarget, privateInvestigatorWatchButton, "Watch");
                return CachedPlayer.LocalPlayer.PlayerControl.CanMove && PrivateInvestigator.currentTarget != null && PrivateInvestigator.watching == null;
            },
            () => { PrivateInvestigator.watching = null; },
            PrivateInvestigator.getButtonSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F
        );

        vampireKillButton = new CustomButton(
            () =>
            {
                if (checkAndDoVetKill(Vampire.currentTarget)) return;
                MurderAttemptResult murder = checkMuderAttempt(Vampire.vampire, Vampire.currentTarget);
                if (murder == MurderAttemptResult.PerformKill)
                {
                    if (Vampire.targetNearGarlic)
                    {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.UncheckedMurderPlayer, SendOption.Reliable, -1);
                        writer.Write(Vampire.vampire.PlayerId);
                        writer.Write(Vampire.currentTarget.PlayerId);
                        writer.Write(Byte.MaxValue);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.uncheckedMurderPlayer(Vampire.vampire.PlayerId, Vampire.currentTarget.PlayerId, Byte.MaxValue);

                        vampireKillButton.HasEffect = false; // Block effect on this click
                        vampireKillButton.Timer = vampireKillButton.MaxTimer;
                    }
                    else
                    {
                        Vampire.bitten = Vampire.currentTarget;
                        // Notify players about bitten
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.VampireSetBitten, SendOption.Reliable, -1);
                        writer.Write(Vampire.bitten.PlayerId);
                        writer.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.vampireSetBitten(Vampire.bitten.PlayerId, 0);

                        byte lastTimer = (byte)Vampire.delay;
                        FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(Vampire.delay, new Action<float>((p) =>
                        { // Delayed action
                            if (p <= 1f)
                            {
                                byte timer = (byte)vampireKillButton.Timer;
                                if (timer != lastTimer)
                                {
                                    lastTimer = timer;
                                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ShareGhostInfo, SendOption.Reliable, -1);
                                    writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                                    writer.Write((byte)RPCProcedure.GhostInfoTypes.VampireTimer);
                                    writer.Write(timer);
                                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                                }
                            }
                            if (p == 1f)
                            {
                                // Perform kill if possible and reset bitten (regardless whether the kill was successful or not)
                                var res = checkMurderAttemptAndKill(Vampire.vampire, Vampire.bitten, showAnimation: false);
                                if (res == MurderAttemptResult.PerformKill)
                                {
                                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.VampireSetBitten, SendOption.Reliable, -1);
                                    writer.Write(byte.MaxValue);
                                    writer.Write(byte.MaxValue);
                                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                                    RPCProcedure.vampireSetBitten(byte.MaxValue, byte.MaxValue);
                                }
                            }
                        })));
                        SoundEffectsManager.play("vampireBite");

                        vampireKillButton.HasEffect = true; // Trigger effect on this click
                    }
                }
                else if (murder == MurderAttemptResult.BlankKill)
                {
                    vampireKillButton.Timer = vampireKillButton.MaxTimer;
                    vampireKillButton.HasEffect = false;
                }
                else if (murder == MurderAttemptResult.BodyGuardKill)
                {
                    checkMuderAttemptAndKill(Vampire.vampire, Vampire.currentTarget);
                }
                else
                {
                    vampireKillButton.HasEffect = false;
                }
            },
            () => { return Vampire.vampire != null && Vampire.vampire == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () =>
            {
                if (Vampire.targetNearGarlic)
                {
                    showTargetNameOnButton(Vampire.currentTarget, vampireKillButton, "KILL");
                }
                else
                {
                    showTargetNameOnButton(Vampire.currentTarget, vampireKillButton, "BITE");
                }
                if (Vampire.targetNearGarlic && Vampire.canKillNearGarlics)
                {
                    vampireKillButton.actionButton.graphic.sprite = __instance.KillButton.graphic.sprite;
                    vampireKillButton.showButtonText = true;
                }
                else
                {
                    vampireKillButton.actionButton.graphic.sprite = Vampire.getButtonSprite();
                    vampireKillButton.showButtonText = false;
                }
                return Vampire.currentTarget != null && CachedPlayer.LocalPlayer.PlayerControl.CanMove && (!Vampire.targetNearGarlic || Vampire.canKillNearGarlics);
            },
            () =>
            {
                vampireKillButton.Timer = vampireKillButton.MaxTimer;
                vampireKillButton.isEffectActive = false;
                vampireKillButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            },
            Vampire.getButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.Q,
            false,
            0f,
            () =>
            {
                vampireKillButton.Timer = vampireKillButton.MaxTimer;
            }
        );

        garlicButton = new CustomButton(
            () =>
            {
                Vampire.localPlacedGarlic = true;
                var pos = CachedPlayer.LocalPlayer.transform.position;
                byte[] buff = new byte[sizeof(float) * 2];
                Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                MessageWriter writer = AmongUsClient.Instance.StartRpc(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.PlaceGarlic, SendOption.Reliable);
                writer.WriteBytesAndSize(buff);
                writer.EndMessage();
                RPCProcedure.placeGarlic(buff);
                SoundEffectsManager.play("garlic");
            },
            () => { return Vampire.garlicButton && !Vampire.localPlacedGarlic && !CachedPlayer.LocalPlayer.Data.IsDead && Vampire.garlicsActive && !HideNSeek.isHideNSeekGM && !PropHunt.isPropHuntGM; },
            () => { return Vampire.garlicButton && CachedPlayer.LocalPlayer.PlayerControl.CanMove && !Vampire.localPlacedGarlic; },
            () => { },
            Vampire.getGarlicButtonSprite(),
            new Vector3(0, -0.06f, 0),
            __instance,
            null,
            true
        );

        portalmakerPlacePortalButton = new CustomButton(
            () =>
            {
                portalmakerPlacePortalButton.Timer = portalmakerPlacePortalButton.MaxTimer;

                var pos = CachedPlayer.LocalPlayer.transform.position;
                byte[] buff = new byte[sizeof(float) * 2];
                Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                MessageWriter writer = AmongUsClient.Instance.StartRpc(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.PlacePortal, SendOption.Reliable);
                writer.WriteBytesAndSize(buff);
                writer.EndMessage();
                RPCProcedure.placePortal(buff);
                SoundEffectsManager.play("tricksterPlaceBox");
            },
            () => { return Portalmaker.portalmaker != null && Portalmaker.portalmaker == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead && Portal.secondPortal == null; },
            () => { return CachedPlayer.LocalPlayer.PlayerControl.CanMove && Portal.secondPortal == null; },
            () => { portalmakerPlacePortalButton.Timer = portalmakerPlacePortalButton.MaxTimer; },
            Portalmaker.getPlacePortalButtonSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F
        );

        usePortalButton = new CustomButton(
            () =>
            {
                bool didTeleport = false;
                Vector3 exit = Portal.findExit(CachedPlayer.LocalPlayer.transform.position);
                Vector3 entry = Portal.findEntry(CachedPlayer.LocalPlayer.transform.position);

                bool portalMakerSoloTeleport = !Portal.locationNearEntry(CachedPlayer.LocalPlayer.transform.position);
                if (portalMakerSoloTeleport)
                {
                    exit = Portal.firstPortal.portalGameObject.transform.position;
                    entry = CachedPlayer.LocalPlayer.transform.position;
                }

                CachedPlayer.LocalPlayer.NetTransform.RpcSnapTo(entry);

                if (!CachedPlayer.LocalPlayer.Data.IsDead)
                {  // Ghosts can portal too, but non-blocking and only with a local animation
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.UsePortal, SendOption.Reliable, -1);
                    writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                    writer.Write(portalMakerSoloTeleport ? (byte)1 : (byte)0);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                }
                RPCProcedure.usePortal(CachedPlayer.LocalPlayer.PlayerId, portalMakerSoloTeleport ? (byte)1 : (byte)0);
                usePortalButton.Timer = usePortalButton.MaxTimer;
                portalmakerMoveToPortalButton.Timer = usePortalButton.MaxTimer;
                SoundEffectsManager.play("portalUse");
                FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(Portal.teleportDuration, new Action<float>((p) =>
                { // Delayed action
                    CachedPlayer.LocalPlayer.PlayerControl.moveable = false;
                    CachedPlayer.LocalPlayer.NetTransform.Halt();
                    if (p >= 0.5f && p <= 0.53f && !didTeleport && !MeetingHud.Instance)
                    {
                        if (SubmergedCompatibility.IsSubmerged)
                        {
                            SubmergedCompatibility.ChangeFloor(exit.y > -7);
                        }
                        CachedPlayer.LocalPlayer.NetTransform.RpcSnapTo(exit);
                        didTeleport = true;
                    }
                    if (p == 1f)
                    {
                        CachedPlayer.LocalPlayer.PlayerControl.moveable = true;
                    }
                })));
            },
            () =>
            {
                if (CachedPlayer.LocalPlayer.PlayerControl == Portalmaker.portalmaker && Portal.bothPlacedAndEnabled)
                    portalmakerButtonText1.text = Portal.locationNearEntry(CachedPlayer.LocalPlayer.transform.position) || !Portalmaker.canPortalFromAnywhere ? "" : "1. " + Portal.firstPortal.room;
                return Portal.bothPlacedAndEnabled;
            },
            () => { return CachedPlayer.LocalPlayer.PlayerControl.CanMove && (Portal.locationNearEntry(CachedPlayer.LocalPlayer.transform.position) || (Portalmaker.canPortalFromAnywhere && CachedPlayer.LocalPlayer.PlayerControl == Portalmaker.portalmaker)) && !Portal.isTeleporting; },
            () => { usePortalButton.Timer = usePortalButton.MaxTimer; },
            Portalmaker.getUsePortalButtonSprite(),
            new Vector3(0.9f, -0.06f, 0),
            __instance,
            KeyCode.H,
            mirror: true
        );

        portalmakerMoveToPortalButton = new CustomButton(
            () =>
            {
                bool didTeleport = false;
                Vector3 exit = Portal.secondPortal.portalGameObject.transform.position;

                if (!CachedPlayer.LocalPlayer.Data.IsDead)
                {  // Ghosts can portal too, but non-blocking and only with a local animation
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.UsePortal, SendOption.Reliable, -1);
                    writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                    writer.Write((byte)2);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                }
                RPCProcedure.usePortal(CachedPlayer.LocalPlayer.PlayerId, 2);
                usePortalButton.Timer = usePortalButton.MaxTimer;
                portalmakerMoveToPortalButton.Timer = usePortalButton.MaxTimer;
                SoundEffectsManager.play("portalUse");
                FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(Portal.teleportDuration, new Action<float>((p) =>
                { // Delayed action
                    CachedPlayer.LocalPlayer.PlayerControl.moveable = false;
                    CachedPlayer.LocalPlayer.NetTransform.Halt();
                    if (p >= 0.5f && p <= 0.53f && !didTeleport && !MeetingHud.Instance)
                    {
                        if (SubmergedCompatibility.IsSubmerged)
                        {
                            SubmergedCompatibility.ChangeFloor(exit.y > -7);
                        }
                        CachedPlayer.LocalPlayer.NetTransform.RpcSnapTo(exit);
                        didTeleport = true;
                    }
                    if (p == 1f)
                    {
                        CachedPlayer.LocalPlayer.PlayerControl.moveable = true;
                    }
                })));
            },
            () => { return Portalmaker.canPortalFromAnywhere && Portal.bothPlacedAndEnabled && CachedPlayer.LocalPlayer.PlayerControl == Portalmaker.portalmaker; },
            () => { return CachedPlayer.LocalPlayer.PlayerControl.CanMove && !Portal.locationNearEntry(CachedPlayer.LocalPlayer.transform.position) && !Portal.isTeleporting; },
            () => { portalmakerMoveToPortalButton.Timer = usePortalButton.MaxTimer; },
            Portalmaker.getUsePortalButtonSprite(),
            new Vector3(0.9f, 1f, 0),
            __instance,
            KeyCode.J,
            mirror: true
        );


        portalmakerButtonText1 = UnityEngine.Object.Instantiate(usePortalButton.actionButton.cooldownTimerText, usePortalButton.actionButton.cooldownTimerText.transform.parent);
        portalmakerButtonText1.text = "";
        portalmakerButtonText1.enableWordWrapping = false;
        portalmakerButtonText1.transform.localScale = Vector3.one * 0.5f;
        portalmakerButtonText1.transform.localPosition += new Vector3(-0.05f, 0.55f, -1f);

        portalmakerButtonText2 = UnityEngine.Object.Instantiate(portalmakerMoveToPortalButton.actionButton.cooldownTimerText, portalmakerMoveToPortalButton.actionButton.cooldownTimerText.transform.parent);
        portalmakerButtonText2.text = "";
        portalmakerButtonText2.enableWordWrapping = false;
        portalmakerButtonText2.transform.localScale = Vector3.one * 0.5f;
        portalmakerButtonText2.transform.localPosition += new Vector3(-0.05f, 0.55f, -1f);



        // Jackal Sidekick Button
        jackalSidekickButton = new CustomButton(
            () =>
            {
                if (checkAndDoVetKill(Jackal.currentTarget)) return;
                checkWatchFlash(Jackal.currentTarget);
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.JackalCreatesSidekick, SendOption.Reliable, -1);
                writer.Write(Jackal.currentTarget.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.jackalCreatesSidekick(Jackal.currentTarget.PlayerId);
                SoundEffectsManager.play("jackalSidekick");
            },
            () => { return Jackal.canCreateSidekick && Jackal.jackal != null && Jackal.jackal == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () =>
            {
                showTargetNameOnButton(Jackal.currentTarget, jackalSidekickButton, ""); // Show now text since the button already says sidekick
                return Jackal.canCreateSidekick && Jackal.currentTarget != null && CachedPlayer.LocalPlayer.PlayerControl.CanMove;
            },
            () => { jackalSidekickButton.Timer = jackalSidekickButton.MaxTimer; },
            Jackal.getSidekickButtonSprite(),
            CustomButton.ButtonPositions.lowerRowCenter,
            __instance,
            KeyCode.F
        );

        // Jackal Kill
        jackalKillButton = new CustomButton(
            () =>
            {
                if (checkAndDoVetKill(Jackal.currentTarget)) return;
                if (checkMuderAttemptAndKill(Jackal.jackal, Jackal.currentTarget) == MurderAttemptResult.SuppressKill) return;

                jackalKillButton.Timer = jackalKillButton.MaxTimer;
                Jackal.currentTarget = null;
            },
            () => { return Jackal.jackal != null && Jackal.jackal == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () =>
            {
                showTargetNameOnButton(Jackal.currentTarget, jackalKillButton, "KILL");
                return Jackal.currentTarget && CachedPlayer.LocalPlayer.PlayerControl.CanMove;
            },
            () => { jackalKillButton.Timer = jackalKillButton.MaxTimer; },
            __instance.KillButton.graphic.sprite,
            //CustomButton.ButtonPositions.upperRowRight,
            CustomButton.ButtonPositions.upperRowCenter,
            __instance,
            KeyCode.Q
        );

        swooperSwoopButton = new CustomButton(
            () =>
            { /* On Use */
                var invisibleWriter = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId,
                    (byte)CustomRPC.SetSwoop, SendOption.Reliable, -1);
                invisibleWriter.Write(Jackal.jackal.PlayerId);
                invisibleWriter.Write(byte.MinValue);
                AmongUsClient.Instance.FinishRpcImmediately(invisibleWriter);
                RPCProcedure.setSwoop(Jackal.jackal.PlayerId, byte.MinValue);
            },
            () =>
            {
                /* Can See */
                return Jackal.jackal != null && Jackal.canSwoop && Jackal.jackal == CachedPlayer.LocalPlayer.PlayerControl
                       && !CachedPlayer.LocalPlayer.Data.IsDead;
            },
            () =>
            {  /* On Click */
                return Jackal.canSwoop && CachedPlayer.LocalPlayer.PlayerControl.CanMove;
            },
            () =>
            {  /* On Meeting End */
                swooperSwoopButton.Timer = swooperSwoopButton.MaxTimer;
                swooperSwoopButton.isEffectActive = false;
                swooperSwoopButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                Jackal.isInvisable = false;
            },
            Jackal.getSwoopButtonSprite(),
            ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.C,
            true,
            Jackal.duration,
            () => { swooperSwoopButton.Timer = swooperSwoopButton.MaxTimer; }
        );
        // }

        // Sidekick Kill
        sidekickKillButton = new CustomButton(
            () =>
            {
                if (checkAndDoVetKill(Sidekick.currentTarget)) return;
                if (checkMuderAttemptAndKill(Sidekick.sidekick, Sidekick.currentTarget) == MurderAttemptResult.SuppressKill) return;
                sidekickKillButton.Timer = sidekickKillButton.MaxTimer;
                Sidekick.currentTarget = null;
            },
            () => { return Sidekick.canKill && Sidekick.sidekick != null && Sidekick.sidekick == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () =>
            {
                showTargetNameOnButton(Sidekick.currentTarget, sidekickKillButton, "KILL");
                return Sidekick.currentTarget && CachedPlayer.LocalPlayer.PlayerControl.CanMove;
            },
            () => { sidekickKillButton.Timer = sidekickKillButton.MaxTimer; },
            __instance.KillButton.graphic.sprite,
            //CustomButton.ButtonPositions.upperRowRight,
            CustomButton.ButtonPositions.upperRowCenter,
            __instance,
            KeyCode.Q
        );

        minerMineButton = new CustomButton(
            () =>
            { /* On Use */
                minerMineButton.Timer = minerMineButton.MaxTimer;

                var writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.Mine, SendOption.Reliable, -1);
                var pos = CachedPlayer.LocalPlayer.PlayerControl.transform.position;
                byte[] buff = new byte[sizeof(float) * 2];
                Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                var id = getAvailableId();
                writer.Write(id);
                writer.Write(CachedPlayer.LocalPlayer.PlayerId);


                writer.WriteBytesAndSize(buff);


                writer.Write(0.01f);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.Mine(id, Miner.miner, buff, 0.01f);
            },
            () => { /* Can See */ return Miner.miner != null && Miner.miner == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () =>
            {  /* Can Use */
                var hits = Physics2D.OverlapBoxAll(CachedPlayer.LocalPlayer.PlayerControl.transform.position, Miner.VentSize, 0);
                hits = hits.ToArray().Where(c => (c.name.Contains("Vent") || !c.isTrigger) && c.gameObject.layer != 8 && c.gameObject.layer != 5).ToArray();
                return hits.Count == 0 && CachedPlayer.LocalPlayer.PlayerControl.CanMove;
            },
            () =>
            {  /* On Meeting End */
                minerMineButton.Timer = minerMineButton.MaxTimer;
            },
            Miner.getMineButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft, //brb
            __instance,
            KeyCode.V,
            buttonText: "Mine"
        );

        bomberBombButton = new CustomButton(
            () =>
            { /* On Use */
                if (checkAndDoVetKill(Bomber.currentTarget)) return;
                checkWatchFlash(Bomber.currentTarget);
                MessageWriter bombWriter = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.GiveBomb, SendOption.Reliable, -1);
                bombWriter.Write(Bomber.currentTarget.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(bombWriter);
                RPCProcedure.giveBomb(Bomber.currentTarget.PlayerId);
                Bomber.bomber.killTimer = Bomber.bombTimer + Bomber.bombDelay;
                bomberBombButton.Timer = bomberBombButton.MaxTimer;
            },
            () => { /* Can See */ return Bomber.bomber != null && Bomber.bomber == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () => {  /* On Click */ return Bomber.currentTarget && CachedPlayer.LocalPlayer.PlayerControl.CanMove; },
            () =>
            {  /* On Meeting End */
                bomberBombButton.Timer = bomberBombButton.MaxTimer;
                bomberBombButton.isEffectActive = false;
                bomberBombButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                Bomber.hasBombPlayer = null;
            },
            Bomber.getButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft, //brb
            __instance,
            KeyCode.V
        );

        bomberGiveButton = new CustomButton(
            () =>
            {
                /* On Use */

                /* On Use */
                if (!Bomber.canGiveToBomber && Bomber.currentBombTarget == Bomber.bomber)
                {
                    var killWriter = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId,
                        (byte)CustomRPC.UncheckedMurderPlayer, SendOption.Reliable);
                    killWriter.Write(Bomber.bomber.Data.PlayerId);
                    killWriter.Write(Bomber.hasBombPlayer.Data.PlayerId);
                    killWriter.Write(0);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.uncheckedMurderPlayer(Bomber.bomber.Data.PlayerId, Bomber.hasBombPlayer.Data.PlayerId, 0);

                    var clearWriter = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId,
                        (byte)CustomRPC.GiveBomb, SendOption.Reliable);
                    clearWriter.Write(byte.MaxValue);
                    clearWriter.Write(false);
                    AmongUsClient.Instance.FinishRpcImmediately(clearWriter);
                    RPCProcedure.giveBomb(byte.MaxValue);
                    return;
                }

                if (checkAndDoVetKill(Bomber.currentBombTarget)) return;
                if (Bomber.hotPotatoMode)
                {
                    var bombWriter = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId,
                        (byte)CustomRPC.GiveBomb, SendOption.Reliable);
                    bombWriter.Write(Bomber.currentBombTarget.PlayerId);
                    bombWriter.Write(true);
                    AmongUsClient.Instance.FinishRpcImmediately(bombWriter);
                    RPCProcedure.giveBomb(Bomber.currentBombTarget.PlayerId, true);
                }
                else
                {
                    if (checkMurderAttemptAndKill(Bomber.hasBombPlayer, Bomber.currentBombTarget) == MurderAttemptResult.SuppressKill) return;
                    var bombWriter = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId,
                        (byte)CustomRPC.GiveBomb, SendOption.Reliable);
                    bombWriter.Write(byte.MaxValue);
                    bombWriter.Write(false);
                    AmongUsClient.Instance.FinishRpcImmediately(bombWriter);
                    RPCProcedure.giveBomb(byte.MaxValue);
                }
            },
            () => { /* Can See */ return Bomber.bomber != null && Bomber.hasBombPlayer == CachedPlayer.LocalPlayer.PlayerControl && Bomber.bombActive && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () => {  /* Can Click */ return Bomber.currentBombTarget && CachedPlayer.LocalPlayer.PlayerControl.CanMove; },
            () => {  /* On Meeting End */ },
            Bomber.getButtonSprite(),
            //          0, -0.06f, 0
            new Vector3(-4.5f, 1.5f, 0),
            __instance,
            KeyCode.B
        );

        // Werewolf Kill
        werewolfKillButton = new CustomButton(
            () =>
            {
                if (checkAndDoVetKill(Werewolf.currentTarget)) return;
                if (checkMuderAttemptAndKill(Werewolf.werewolf, Werewolf.currentTarget) == MurderAttemptResult.SuppressKill) return;

                werewolfKillButton.Timer = werewolfKillButton.MaxTimer;
                Werewolf.currentTarget = null;
            },
            () => { return Werewolf.werewolf != null && Werewolf.werewolf == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead && Werewolf.canKill; },
            () => { showTargetNameOnButton(Werewolf.currentTarget, werewolfKillButton, "KILL"); return Werewolf.currentTarget && CachedPlayer.LocalPlayer.PlayerControl.CanMove; },
            () => { werewolfKillButton.Timer = werewolfKillButton.MaxTimer; },
            __instance.KillButton.graphic.sprite,
            new Vector3(0, 1f, 0),
            __instance,
            KeyCode.Q
        );

        werewolfRampageButton = new CustomButton(
            () => { Werewolf.canKill = true; Werewolf.hasImpostorVision = true; werewolfKillButton.Timer = 0f; },
            () => { /* Can See */ return Werewolf.werewolf != null && Werewolf.werewolf == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () => {  /* On Click */ return CachedPlayer.LocalPlayer.PlayerControl.CanMove; },
            () =>
            {  /* On Meeting End */
                werewolfRampageButton.Timer = werewolfRampageButton.MaxTimer;
                werewolfRampageButton.isEffectActive = false;
                werewolfRampageButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                Werewolf.canKill = false;
                //  Werewolf.canUseVents = false;
                Werewolf.hasImpostorVision = false;

            },
            Werewolf.getRampageButtonSprite(),
            CustomButton.ButtonPositions.lowerRowRight, //brb
            __instance,
            KeyCode.G,
            true,
            Werewolf.rampageDuration,
            () => { werewolfRampageButton.Timer = werewolfRampageButton.MaxTimer; Werewolf.canKill = false; Werewolf.hasImpostorVision = false; }
        );

        // Eraser erase button
        eraserButton = new CustomButton(
            () =>
            {
                if (checkAndDoVetKill(Eraser.currentTarget)) return;
                checkWatchFlash(Eraser.currentTarget);
                eraserButton.MaxTimer += 10;
                eraserButton.Timer = eraserButton.MaxTimer;

                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.SetFutureErased, SendOption.Reliable, -1);
                writer.Write(Eraser.currentTarget.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.setFutureErased(Eraser.currentTarget.PlayerId);
                SoundEffectsManager.play("eraserErase");
            },
            () => { return Eraser.eraser != null && Eraser.eraser == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () =>
            {
                showTargetNameOnButton(Eraser.currentTarget, eraserButton, "ERASE");
                return CachedPlayer.LocalPlayer.PlayerControl.CanMove && Eraser.currentTarget != null;
            },
            () => { eraserButton.Timer = eraserButton.MaxTimer; },
            Eraser.getButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.F
        );

        placeJackInTheBoxButton = new CustomButton(
            () =>
            {
                placeJackInTheBoxButton.Timer = placeJackInTheBoxButton.MaxTimer;

                var pos = CachedPlayer.LocalPlayer.transform.position;
                byte[] buff = new byte[sizeof(float) * 2];
                Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                MessageWriter writer = AmongUsClient.Instance.StartRpc(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.PlaceJackInTheBox, SendOption.Reliable);
                writer.WriteBytesAndSize(buff);
                writer.EndMessage();
                RPCProcedure.placeJackInTheBox(buff);
                SoundEffectsManager.play("tricksterPlaceBox");
            },
            () => { return Trickster.trickster != null && Trickster.trickster == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead && !JackInTheBox.hasJackInTheBoxLimitReached(); },
            () => { return CachedPlayer.LocalPlayer.PlayerControl.CanMove && !JackInTheBox.hasJackInTheBoxLimitReached(); },
            () => { placeJackInTheBoxButton.Timer = placeJackInTheBoxButton.MaxTimer; },
            Trickster.getPlaceBoxButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.F
        );

        lightsOutButton = new CustomButton(
            () =>
            {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.LightsOut, SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.lightsOut();
                SoundEffectsManager.play("lighterLight");
            },
            () =>
            {
                return Trickster.trickster != null && Trickster.trickster == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead
                                                       && JackInTheBox.hasJackInTheBoxLimitReached() && JackInTheBox.boxesConvertedToVents;
            },
            () => { return CachedPlayer.LocalPlayer.PlayerControl.CanMove && JackInTheBox.hasJackInTheBoxLimitReached() && JackInTheBox.boxesConvertedToVents; },
            () =>
            {
                lightsOutButton.Timer = lightsOutButton.MaxTimer;
                lightsOutButton.isEffectActive = false;
                lightsOutButton.actionButton.graphic.color = Palette.EnabledColor;
            },
            Trickster.getLightsOutButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.F,
            true,
            Trickster.lightsOutDuration,
            () =>
            {
                lightsOutButton.Timer = lightsOutButton.MaxTimer;
                SoundEffectsManager.play("lighterLight");
            }
        );

        // Cleaner Clean
        cleanerCleanButton = new CustomButton(
            () =>
            {
                foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(CachedPlayer.LocalPlayer.PlayerControl.GetTruePosition(), CachedPlayer.LocalPlayer.PlayerControl.MaxReportDistance, Constants.PlayersOnlyMask))
                {
                    if (collider2D.tag == "DeadBody")
                    {
                        DeadBody component = collider2D.GetComponent<DeadBody>();
                        if (component && !component.Reported)
                        {
                            Vector2 truePosition = CachedPlayer.LocalPlayer.PlayerControl.GetTruePosition();
                            Vector2 truePosition2 = component.TruePosition;
                            if (Vector2.Distance(truePosition2, truePosition) <= CachedPlayer.LocalPlayer.PlayerControl.MaxReportDistance && CachedPlayer.LocalPlayer.PlayerControl.CanMove && !PhysicsHelpers.AnythingBetween(truePosition, truePosition2, Constants.ShipAndObjectsMask, false))
                            {
                                NetworkedPlayerInfo playerInfo = GameData.Instance.GetPlayerById(component.ParentId);

                                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.CleanBody, SendOption.Reliable, -1);
                                writer.Write(playerInfo.PlayerId);
                                writer.Write(Cleaner.cleaner.PlayerId);
                                AmongUsClient.Instance.FinishRpcImmediately(writer);
                                RPCProcedure.cleanBody(playerInfo.PlayerId, Cleaner.cleaner.PlayerId);

                                Cleaner.cleaner.killTimer = cleanerCleanButton.Timer = cleanerCleanButton.MaxTimer;
                                SoundEffectsManager.play("cleanerClean");
                                break;
                            }
                        }
                    }
                }
            },
            () => { return Cleaner.cleaner != null && Cleaner.cleaner == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () => { return __instance.ReportButton.graphic.color == Palette.EnabledColor && CachedPlayer.LocalPlayer.PlayerControl.CanMove; },
            () => { cleanerCleanButton.Timer = cleanerCleanButton.MaxTimer; },
            Cleaner.getButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.F
        );

        // Yoyo button
        yoyoButton = new CustomButton(
            () =>
            {
                var pos = CachedPlayer.LocalPlayer.transform.position;
                byte[] buff = new byte[sizeof(float) * 2];
                Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                if (Yoyo.markedLocation == null)
                {
                    MessageWriter writer = AmongUsClient.Instance.StartRpc(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.YoyoMarkLocation, SendOption.Reliable);
                    writer.WriteBytesAndSize(buff);
                    writer.EndMessage();
                    RPCProcedure.yoyoMarkLocation(buff);
                    SoundEffectsManager.play("tricksterPlaceBox");
                    yoyoButton.Sprite = Yoyo.getBlinkButtonSprite();
                    yoyoButton.Timer = 10f;
                    yoyoButton.HasEffect = false;
                    yoyoButton.buttonText = "Blink";
                }
                else
                {
                    // Jump to location
                    var exit = (Vector3)Yoyo.markedLocation;
                    if (SubmergedCompatibility.IsSubmerged)
                    {
                        SubmergedCompatibility.ChangeFloor(exit.y > -7);
                    }
                    MessageWriter writer = AmongUsClient.Instance.StartRpc(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.YoyoBlink, SendOption.Reliable);
                    writer.Write(Byte.MaxValue);
                    writer.WriteBytesAndSize(buff);
                    writer.EndMessage();
                    RPCProcedure.yoyoBlink(true, buff);
                    yoyoButton.EffectDuration = Yoyo.blinkDuration;
                    yoyoButton.Timer = 10f;
                    yoyoButton.HasEffect = true;
                    yoyoButton.buttonText = "Returning...";
                    SoundEffectsManager.play("morphlingMorph");
                }
            },
            () => { return Yoyo.yoyo != null && Yoyo.yoyo == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () => { return CachedPlayer.LocalPlayer.PlayerControl.CanMove; },
            () =>
            {
                if (Yoyo.markStaysOverMeeting)
                {
                    yoyoButton.Timer = 10f;
                }
                else
                {
                    Yoyo.markedLocation = null;
                    yoyoButton.Timer = yoyoButton.MaxTimer;
                    yoyoButton.Sprite = Yoyo.getMarkButtonSprite();
                    yoyoButton.buttonText = "Mark Location";
                }
            },
            Yoyo.getMarkButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.F,
            false,
            Yoyo.blinkDuration,
            () =>
            {
                if (TransportationToolPatches.isUsingTransportation(Yoyo.yoyo))
                {
                    yoyoButton.Timer = 0.5f;
                    yoyoButton.DeputyTimer = 0.5f;
                    yoyoButton.isEffectActive = true;
                    yoyoButton.actionButton.cooldownTimerText.color = new Color(0F, 0.8F, 0F);
                    return;
                }
                else if (Yoyo.yoyo.inVent)
                {
                    __instance.ImpostorVentButton.DoClick();
                }
                // jump back!
                var pos = CachedPlayer.LocalPlayer.transform.position;
                byte[] buff = new byte[sizeof(float) * 2];
                Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));
                var exit = (Vector3)Yoyo.markedLocation;
                if (SubmergedCompatibility.IsSubmerged)
                {
                    SubmergedCompatibility.ChangeFloor(exit.y > -7);
                }
                MessageWriter writer = AmongUsClient.Instance.StartRpc(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.YoyoBlink, SendOption.Reliable);
                writer.Write((byte)0);
                writer.WriteBytesAndSize(buff);
                writer.EndMessage();
                RPCProcedure.yoyoBlink(false, buff);
                yoyoButton.Timer = yoyoButton.MaxTimer;
                yoyoButton.isEffectActive = false;
                yoyoButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                yoyoButton.HasEffect = false;
                yoyoButton.Sprite = Yoyo.getMarkButtonSprite();
                yoyoButton.buttonText = "Mark Location";
                SoundEffectsManager.play("morphlingMorph");
                if (Minigame.Instance)
                {
                    Minigame.Instance.Close();
                }
            },
            buttonText: "Mark Location"
        );

        yoyoAdminTableButton = new CustomButton(
           () =>
           {
               if (!MapBehaviour.Instance || !MapBehaviour.Instance.isActiveAndEnabled)
               {
                   HudManager __instance = FastDestroyableSingleton<HudManager>.Instance;
                   __instance.InitMap();
                   MapBehaviour.Instance.ShowCountOverlay(allowedToMove: true, showLivePlayerPosition: true, includeDeadBodies: true);
               }
           },
           () => { return Yoyo.yoyo != null && Yoyo.yoyo == CachedPlayer.LocalPlayer.PlayerControl && Yoyo.hasAdminTable && !CachedPlayer.LocalPlayer.Data.IsDead; },
           () =>
           {
               return true;
           },
           () =>
           {
               yoyoAdminTableButton.Timer = yoyoAdminTableButton.MaxTimer;
               yoyoAdminTableButton.isEffectActive = false;
               yoyoAdminTableButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
           },
           Hacker.getAdminSprite(),
           CustomButton.ButtonPositions.lowerRowCenter,
           __instance,
           KeyCode.G,
           true,
           0f,
           () =>
           {
               yoyoAdminTableButton.Timer = yoyoAdminTableButton.MaxTimer;
               if (MapBehaviour.Instance && MapBehaviour.Instance.isActiveAndEnabled) MapBehaviour.Instance.Close();
           },
           GameOptionsManager.Instance.currentNormalGameOptions.MapId == 3,
           "ADMIN"
       );

        // Cleaner Clean
        undertakerDragButton = new CustomButton(
            () =>
            {
                if (Undertaker.deadBodyDraged == null)
                {
                    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(CachedPlayer.LocalPlayer.PlayerControl.GetTruePosition(), CachedPlayer.LocalPlayer.PlayerControl.MaxReportDistance, Constants.PlayersOnlyMask))
                    {
                        if (collider2D.tag == "DeadBody")
                        {
                            DeadBody deadBody = collider2D.GetComponent<DeadBody>();
                            if (deadBody && !deadBody.Reported)
                            {
                                Vector2 playerPosition = CachedPlayer.LocalPlayer.PlayerControl.GetTruePosition();
                                Vector2 deadBodyPosition = deadBody.TruePosition;
                                if (Vector2.Distance(deadBodyPosition, playerPosition) <= CachedPlayer.LocalPlayer.PlayerControl.MaxReportDistance && CachedPlayer.LocalPlayer.PlayerControl.CanMove && !PhysicsHelpers.AnythingBetween(playerPosition, deadBodyPosition, Constants.ShipAndObjectsMask, false) && !Undertaker.isDraging)
                                {
                                    NetworkedPlayerInfo playerInfo = GameData.Instance.GetPlayerById(deadBody.ParentId);
                                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.DragBody, SendOption.Reliable, -1);
                                    writer.Write(playerInfo.PlayerId);
                                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                                    RPCProcedure.dragBody(playerInfo.PlayerId);
                                    Undertaker.deadBodyDraged = deadBody;
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    var writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.DropBody, SendOption.Reliable, -1);
                    writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    Undertaker.deadBodyDraged = null;
                }

            },
            () => { return Undertaker.undertaker != null && Undertaker.undertaker == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () =>
            {
                if (Undertaker.deadBodyDraged != null) return true;
                else
                {
                    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(CachedPlayer.LocalPlayer.PlayerControl.GetTruePosition(), CachedPlayer.LocalPlayer.PlayerControl.MaxReportDistance, Constants.PlayersOnlyMask))
                    {
                        if (collider2D.tag == "DeadBody")
                        {
                            DeadBody deadBody = collider2D.GetComponent<DeadBody>();
                            Vector2 deadBodyPosition = deadBody.TruePosition;
                            deadBodyPosition.x -= 0.2f;
                            deadBodyPosition.y -= 0.2f;
                            return CachedPlayer.LocalPlayer.PlayerControl.CanMove && Vector2.Distance(CachedPlayer.LocalPlayer.PlayerControl.GetTruePosition(), deadBodyPosition) < 0.80f;
                        }
                    }
                    return false;
                }

            },
            //() => { return ((__instance.ReportButton.renderer.color == Palette.EnabledColor && CachedPlayer.LocalPlayer.PlayerControl.CanMove) || Undertaker.deadBodyDraged != null); },
            () => { },
            Undertaker.getButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft, //brb
            __instance,
            KeyCode.F,
            true,
            0f,
            () => { }
        );

        // Warlock curse
        warlockCurseButton = new CustomButton(
            () =>
            {
                if (Warlock.curseVictim == null)
                {
                    if (checkAndDoVetKill(Warlock.currentTarget)) return;
                    checkWatchFlash(Warlock.currentTarget);
                    // Apply Curse
                    Warlock.curseVictim = Warlock.currentTarget;
                    warlockCurseButton.Sprite = Warlock.getCurseKillButtonSprite();
                    warlockCurseButton.Timer = 1f;
                    SoundEffectsManager.play("warlockCurse");

                    // Ghost Info
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ShareGhostInfo, SendOption.Reliable, -1);
                    writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                    writer.Write((byte)RPCProcedure.GhostInfoTypes.WarlockTarget);
                    writer.Write(Warlock.curseVictim.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);

                }
                else if (Warlock.curseVictim != null && Warlock.curseVictimTarget != null)
                {
                    MurderAttemptResult murder = checkMurderAttemptAndKill(Warlock.warlock, Warlock.curseVictimTarget, showAnimation: false);
                    if (murder == MurderAttemptResult.SuppressKill) return;

                    // If blanked or killed
                    if (Warlock.rootTime > 0)
                    {
                        AntiTeleport.position = CachedPlayer.LocalPlayer.transform.position;
                        CachedPlayer.LocalPlayer.PlayerControl.moveable = false;
                        CachedPlayer.LocalPlayer.NetTransform.Halt(); // Stop current movement so the warlock is not just running straight into the next object
                        FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(Warlock.rootTime, new Action<float>((p) =>
                        { // Delayed action
                            if (p == 1f)
                            {
                                CachedPlayer.LocalPlayer.PlayerControl.moveable = true;
                            }
                        })));
                    }

                    Warlock.curseVictim = null;
                    Warlock.curseVictimTarget = null;
                    warlockCurseButton.Sprite = Warlock.getCurseButtonSprite();
                    Warlock.warlock.killTimer = warlockCurseButton.Timer = warlockCurseButton.MaxTimer;

                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ShareGhostInfo, SendOption.Reliable, -1);
                    writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                    writer.Write((byte)RPCProcedure.GhostInfoTypes.WarlockTarget);
                    writer.Write(Byte.MaxValue); // This will set it to null!
                    AmongUsClient.Instance.FinishRpcImmediately(writer);

                }
            },
            () => { return Warlock.warlock != null && Warlock.warlock == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () =>
            {
                if (Warlock.curseVictim != null)
                    showTargetNameOnButton(Warlock.currentTarget, warlockCurseButton, "KILL");
                else
                    showTargetNameOnButton(Warlock.currentTarget, warlockCurseButton, "CURSE");
                return ((Warlock.curseVictim == null && Warlock.currentTarget != null) || (Warlock.curseVictim != null && Warlock.curseVictimTarget != null)) && CachedPlayer.LocalPlayer.PlayerControl.CanMove;
            },
            () =>
            {
                warlockCurseButton.Timer = warlockCurseButton.MaxTimer;
                warlockCurseButton.Sprite = Warlock.getCurseButtonSprite();
                Warlock.curseVictim = null;
                Warlock.curseVictimTarget = null;
            },
            Warlock.getCurseButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.F
        );

        // Security Guard button
        securityGuardButton = new CustomButton(
            () =>
            {
                if (SecurityGuard.ventTarget != null)
                { // Seal vent
                    MessageWriter writer = AmongUsClient.Instance.StartRpc(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.SealVent, SendOption.Reliable);
                    writer.WritePacked(SecurityGuard.ventTarget.Id);
                    writer.EndMessage();
                    RPCProcedure.sealVent(SecurityGuard.ventTarget.Id);
                    SecurityGuard.ventTarget = null;

                }
                else if (!IsMira && !IsFungle && !SubmergedCompatibility.IsSubmerged)
                { // Place camera if there's no vent and it's not MiraHQ or Submerged
                    var pos = CachedPlayer.LocalPlayer.transform.position;
                    byte[] buff = new byte[sizeof(float) * 2];
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                    MessageWriter writer = AmongUsClient.Instance.StartRpc(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.PlaceCamera, SendOption.Reliable);
                    writer.WriteBytesAndSize(buff);
                    writer.EndMessage();
                    RPCProcedure.placeCamera(buff);
                }
                SoundEffectsManager.play("securityGuardPlaceCam");  // Same sound used for both types (cam or vent)!
                securityGuardButton.Timer = securityGuardButton.MaxTimer;
            },
            () => { return SecurityGuard.securityGuard != null && SecurityGuard.securityGuard == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead && SecurityGuard.remainingScrews >= Mathf.Min(SecurityGuard.ventPrice, SecurityGuard.camPrice); },
            () =>
            {
                securityGuardButton.actionButton.graphic.sprite = (SecurityGuard.ventTarget == null && !IsMira && !IsFungle && !SubmergedCompatibility.IsSubmerged) ? SecurityGuard.getPlaceCameraButtonSprite() : SecurityGuard.getCloseVentButtonSprite();
                if (securityGuardButtonScrewsText != null) securityGuardButtonScrewsText.text = $"{SecurityGuard.remainingScrews}/{SecurityGuard.totalScrews}";

                if (SecurityGuard.ventTarget != null)
                    return SecurityGuard.remainingScrews >= SecurityGuard.ventPrice && CachedPlayer.LocalPlayer.PlayerControl.CanMove;
                return !IsMira && !IsFungle && !SubmergedCompatibility.IsSubmerged && SecurityGuard.remainingScrews >= SecurityGuard.camPrice && CachedPlayer.LocalPlayer.PlayerControl.CanMove;
            },
            () => { securityGuardButton.Timer = securityGuardButton.MaxTimer; },
            SecurityGuard.getPlaceCameraButtonSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F
        );

        // Security Guard button screws counter
        securityGuardButtonScrewsText = UnityEngine.Object.Instantiate(securityGuardButton.actionButton.cooldownTimerText, securityGuardButton.actionButton.cooldownTimerText.transform.parent);
        securityGuardButtonScrewsText.text = "";
        securityGuardButtonScrewsText.enableWordWrapping = false;
        securityGuardButtonScrewsText.transform.localScale = Vector3.one * 0.5f;
        securityGuardButtonScrewsText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

        securityGuardCamButton = new CustomButton(
            () =>
            {
                if (!IsMira)
                {
                    if (SecurityGuard.minigame == null)
                    {
                        byte mapId = GameOptionsManager.Instance.currentNormalGameOptions.MapId;
                        var e = UnityEngine.Object.FindObjectsOfType<SystemConsole>().FirstOrDefault(x => x.gameObject.name.Contains("Surv_Panel") || x.name.Contains("Cam") || x.name.Contains("BinocularsSecurityConsole"));
                        if (IsSkeld || mapId == 3) e = UnityEngine.Object.FindObjectsOfType<SystemConsole>().FirstOrDefault(x => x.gameObject.name.Contains("SurvConsole"));
                        else if (IsAirship) e = UnityEngine.Object.FindObjectsOfType<SystemConsole>().FirstOrDefault(x => x.gameObject.name.Contains("task_cams"));
                        if (e == null || Camera.main == null) return;
                        SecurityGuard.minigame = UnityEngine.Object.Instantiate(e.MinigamePrefab, Camera.main.transform, false);
                    }
                    SecurityGuard.minigame.transform.SetParent(Camera.main.transform, false);
                    SecurityGuard.minigame.transform.localPosition = new Vector3(0.0f, 0.0f, -50f);
                    SecurityGuard.minigame.Begin(null);
                }
                else
                {
                    if (SecurityGuard.minigame == null)
                    {
                        var e = UnityEngine.Object.FindObjectsOfType<SystemConsole>().FirstOrDefault(x => x.gameObject.name.Contains("SurvLogConsole"));
                        if (e == null || Camera.main == null) return;
                        SecurityGuard.minigame = UnityEngine.Object.Instantiate(e.MinigamePrefab, Camera.main.transform, false);
                    }
                    SecurityGuard.minigame.transform.SetParent(Camera.main.transform, false);
                    SecurityGuard.minigame.transform.localPosition = new Vector3(0.0f, 0.0f, -50f);
                    SecurityGuard.minigame.Begin(null);
                }
                SecurityGuard.charges--;

                if (SecurityGuard.cantMove) CachedPlayer.LocalPlayer.PlayerControl.moveable = false;
                CachedPlayer.LocalPlayer.NetTransform.Halt(); // Stop current movement 
            },
            () =>
            {
                return SecurityGuard.securityGuard != null && SecurityGuard.securityGuard == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead && SecurityGuard.remainingScrews < Mathf.Min(SecurityGuard.ventPrice, SecurityGuard.camPrice)
                           && !SubmergedCompatibility.IsSubmerged;
            },
            () =>
            {
                if (securityGuardChargesText != null) securityGuardChargesText.text = $"{SecurityGuard.charges} / {SecurityGuard.maxCharges}";
                securityGuardCamButton.actionButton.graphic.sprite = IsMira ? SecurityGuard.getLogSprite() : SecurityGuard.getCamSprite();
                securityGuardCamButton.actionButton.OverrideText(IsMira ? "DOORLOG" : "SECURITY");
                return CachedPlayer.LocalPlayer.PlayerControl.CanMove && SecurityGuard.charges > 0;
            },
            () =>
            {
                securityGuardCamButton.Timer = securityGuardCamButton.MaxTimer;
                securityGuardCamButton.isEffectActive = false;
                securityGuardCamButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            },
            SecurityGuard.getCamSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.Q,
            true,
            0f,
            () =>
            {
                securityGuardCamButton.Timer = securityGuardCamButton.MaxTimer;
                if (Minigame.Instance)
                {
                    SecurityGuard.minigame.ForceClose();
                }
                CachedPlayer.LocalPlayer.PlayerControl.moveable = true;
            },
            false,
            IsMira ? "DOORLOG" : "SECURITY"
        );

        // Security Guard cam button charges
        securityGuardChargesText = UnityEngine.Object.Instantiate(securityGuardCamButton.actionButton.cooldownTimerText, securityGuardCamButton.actionButton.cooldownTimerText.transform.parent);
        securityGuardChargesText.text = "";
        securityGuardChargesText.enableWordWrapping = false;
        securityGuardChargesText.transform.localScale = Vector3.one * 0.5f;
        securityGuardChargesText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

        // Arsonist button
        arsonistButton = new CustomButton(
            () =>
            {
                bool dousedEveryoneAlive = Arsonist.dousedEveryoneAlive();
                if (dousedEveryoneAlive)
                {
                    MessageWriter winWriter = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ArsonistWin, SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(winWriter);
                    RPCProcedure.arsonistWin();
                    arsonistButton.HasEffect = false;
                }
                else if (Arsonist.currentTarget != null)
                {
                    if (checkAndDoVetKill(Arsonist.currentTarget)) return;
                    checkWatchFlash(Arsonist.currentTarget);
                    Arsonist.douseTarget = Arsonist.currentTarget;
                    arsonistButton.HasEffect = true;
                    SoundEffectsManager.play("arsonistDouse");
                }
            },
            () => { return Arsonist.arsonist != null && Arsonist.arsonist == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () =>
            {
                bool dousedEveryoneAlive = Arsonist.dousedEveryoneAlive();
                if (!dousedEveryoneAlive)
                    showTargetNameOnButton(Arsonist.currentTarget, arsonistButton, "");
                if (dousedEveryoneAlive) arsonistButton.actionButton.graphic.sprite = Arsonist.getIgniteSprite();

                if (arsonistButton.isEffectActive && Arsonist.douseTarget != Arsonist.currentTarget)
                {
                    Arsonist.douseTarget = null;
                    arsonistButton.Timer = 0f;
                    arsonistButton.isEffectActive = false;
                }

                return CachedPlayer.LocalPlayer.PlayerControl.CanMove && (dousedEveryoneAlive || Arsonist.currentTarget != null);
            },
            () =>
            {
                arsonistButton.Timer = arsonistButton.MaxTimer;
                arsonistButton.isEffectActive = false;
                Arsonist.douseTarget = null;
            },
            Arsonist.getDouseSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F,
            true,
            Arsonist.duration,
            () =>
            {
                if (Arsonist.douseTarget != null) Arsonist.dousedPlayers.Add(Arsonist.douseTarget);

                arsonistButton.Timer = Arsonist.dousedEveryoneAlive() ? 0 : arsonistButton.MaxTimer;

                foreach (PlayerControl p in Arsonist.dousedPlayers)
                {
                    if (TORMapOptions.playerIcons.ContainsKey(p.PlayerId))
                    {
                        TORMapOptions.playerIcons[p.PlayerId].setSemiTransparent(false);
                    }
                }

                // Ghost Info
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ShareGhostInfo, SendOption.Reliable, -1);
                writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                writer.Write((byte)RPCProcedure.GhostInfoTypes.ArsonistDouse);
                writer.Write(Arsonist.douseTarget.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);

                Arsonist.douseTarget = null;
            }
        );

        // doomsayer Shield
        doomsayerButton = new CustomButton(
            () =>
            {
                if (checkAndDoVetKill(Doomsayer.currentTarget)) return;

                doomsayerButton.Timer = doomsayerButton.MaxTimer;
                SoundEffectsManager.play("knockKnock");
            },
            () =>
            {
                return Doomsayer.doomsayer != null && Doomsayer.doomsayer == CachedPlayer.LocalPlayer.PlayerControl &&
                       !CachedPlayer.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                showTargetNameOnButton(Doomsayer.currentTarget, doomsayerButton, "Reveal");
                return CachedPlayer.LocalPlayer.PlayerControl.CanMove && Doomsayer.currentTarget != null;
            },
            () => { doomsayerButton.Timer = doomsayerButton.MaxTimer; },
            Doomsayer.getButtonSprite(),
            ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F,
            true,
            0f,
            () =>
            {
                doomsayerButton.Timer = doomsayerButton.MaxTimer;
                var msg = Doomsayer.GetInfo(Doomsayer.currentTarget);
                FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(CachedPlayer.LocalPlayer.PlayerControl, $"{msg}");

                // Ghost Info
                var writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId,
                    (byte)CustomRPC.ShareGhostInfo, SendOption.Reliable);
                writer.Write(Doomsayer.currentTarget.PlayerId);
                writer.Write((byte)RPCProcedure.GhostInfoTypes.MediumInfo);
                writer.Write(msg);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
        );

        // Vulture Eat
        vultureEatButton = new CustomButton(
            () =>
            {
                foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(CachedPlayer.LocalPlayer.PlayerControl.GetTruePosition(), CachedPlayer.LocalPlayer.PlayerControl.MaxReportDistance, Constants.PlayersOnlyMask))
                {
                    if (collider2D.tag == "DeadBody")
                    {
                        DeadBody component = collider2D.GetComponent<DeadBody>();
                        if (component && !component.Reported)
                        {
                            Vector2 truePosition = CachedPlayer.LocalPlayer.PlayerControl.GetTruePosition();
                            Vector2 truePosition2 = component.TruePosition;
                            if (Vector2.Distance(truePosition2, truePosition) <= CachedPlayer.LocalPlayer.PlayerControl.MaxReportDistance && CachedPlayer.LocalPlayer.PlayerControl.CanMove && !PhysicsHelpers.AnythingBetween(truePosition, truePosition2, Constants.ShipAndObjectsMask, false))
                            {
                                NetworkedPlayerInfo playerInfo = GameData.Instance.GetPlayerById(component.ParentId);

                                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.CleanBody, SendOption.Reliable, -1);
                                writer.Write(playerInfo.PlayerId);
                                writer.Write(Vulture.vulture.PlayerId);
                                AmongUsClient.Instance.FinishRpcImmediately(writer);
                                RPCProcedure.cleanBody(playerInfo.PlayerId, Vulture.vulture.PlayerId);

                                Vulture.cooldown = vultureEatButton.Timer = vultureEatButton.MaxTimer;
                                SoundEffectsManager.play("vultureEat");
                                break;
                            }
                        }
                    }
                }
            },
            () => { return Vulture.vulture != null && Vulture.vulture == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () => { return __instance.ReportButton.graphic.color == Palette.EnabledColor && CachedPlayer.LocalPlayer.PlayerControl.CanMove; },
            () => { vultureEatButton.Timer = vultureEatButton.MaxTimer; },
            Vulture.getButtonSprite(),
            CustomButton.ButtonPositions.lowerRowCenter,
            __instance,
            KeyCode.F
        );

        amnisiacRememberButton = new CustomButton(
            () =>
            {
                foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(CachedPlayer.LocalPlayer.PlayerControl.GetTruePosition(), CachedPlayer.LocalPlayer.PlayerControl.MaxReportDistance, Constants.PlayersOnlyMask))
                {
                    if (collider2D.tag == "DeadBody")
                    {
                        DeadBody component = collider2D.GetComponent<DeadBody>();
                        if (component && !component.Reported)
                        {
                            Vector2 truePosition = CachedPlayer.LocalPlayer.PlayerControl.GetTruePosition();
                            Vector2 truePosition2 = component.TruePosition;
                            if (Vector2.Distance(truePosition2, truePosition) <= CachedPlayer.LocalPlayer.PlayerControl.MaxReportDistance && CachedPlayer.LocalPlayer.PlayerControl.CanMove && !PhysicsHelpers.AnythingBetween(truePosition, truePosition2, Constants.ShipAndObjectsMask, false))
                            {
                                NetworkedPlayerInfo playerInfo = GameData.Instance.GetPlayerById(component.ParentId);

                                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.AmnisiacTakeRole, SendOption.Reliable, -1);
                                writer.Write(playerInfo.PlayerId);
                                AmongUsClient.Instance.FinishRpcImmediately(writer);
                                RPCProcedure.amnisiacTakeRole(playerInfo.PlayerId);
                                break;
                            }
                        }
                    }
                }
            },
            () => { return Amnisiac.amnisiac != null && Amnisiac.amnisiac == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () => { return __instance.ReportButton.graphic.color == Palette.EnabledColor && CachedPlayer.LocalPlayer.PlayerControl.CanMove; },
            () => { amnisiacRememberButton.Timer = 0f; },
            Amnisiac.getButtonSprite(),
            CustomButton.ButtonPositions.lowerRowRight, //brb
            __instance,
            KeyCode.F
        );

        // Medium button
        mediumButton = new CustomButton(
            () =>
            {
                if (Medium.target != null)
                {
                    Medium.soulTarget = Medium.target;
                    mediumButton.HasEffect = true;
                    SoundEffectsManager.play("mediumAsk");
                }
            },
            () => { return Medium.medium != null && Medium.medium == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () =>
            {
                if (mediumButton.isEffectActive && Medium.target != Medium.soulTarget)
                {
                    Medium.soulTarget = null;
                    mediumButton.Timer = 0f;
                    mediumButton.isEffectActive = false;
                }
                return Medium.target != null && CachedPlayer.LocalPlayer.PlayerControl.CanMove;
            },
            () =>
            {
                mediumButton.Timer = mediumButton.MaxTimer;
                mediumButton.isEffectActive = false;
                Medium.soulTarget = null;
            },
            Medium.getQuestionSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F,
            true,
            Medium.duration,
            () =>
            {
                mediumButton.Timer = mediumButton.MaxTimer;
                if (Medium.target == null || Medium.target.player == null) return;
                string msg = Medium.getInfo(Medium.target.player, Medium.target.killerIfExisting);
                FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(CachedPlayer.LocalPlayer.PlayerControl, msg);

                // Ghost Info
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ShareGhostInfo, SendOption.Reliable, -1);
                writer.Write(Medium.target.player.PlayerId);
                writer.Write((byte)RPCProcedure.GhostInfoTypes.MediumInfo);
                writer.Write(msg);
                AmongUsClient.Instance.FinishRpcImmediately(writer);

                // Remove soul
                if (Medium.oneTimeUse)
                {
                    float closestDistance = float.MaxValue;
                    SpriteRenderer target = null;

                    foreach ((DeadPlayer db, Vector3 ps) in Medium.deadBodies)
                    {
                        if (db == Medium.target)
                        {
                            Tuple<DeadPlayer, Vector3> deadBody = Tuple.Create(db, ps);
                            Medium.deadBodies.Remove(deadBody);
                            break;
                        }

                    }
                    foreach (SpriteRenderer rend in Medium.souls)
                    {
                        float distance = Vector2.Distance(rend.transform.position, CachedPlayer.LocalPlayer.PlayerControl.GetTruePosition());
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            target = rend;
                        }
                    }

                    FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(5f, new Action<float>((p) =>
                    {
                        if (target != null)
                        {
                            var tmp = target.color;
                            tmp.a = Mathf.Clamp01(1 - p);
                            target.color = tmp;
                        }
                        if (p == 1f && target != null && target.gameObject != null) UnityEngine.Object.Destroy(target.gameObject);
                    })));

                    Medium.souls.Remove(target);
                }
                SoundEffectsManager.stop("mediumAsk");
            }
        );

        // Pursuer button
        pursuerButton = new CustomButton(
            () =>
            {
                if (Pursuer.target != null)
                {
                    if (checkAndDoVetKill(Pursuer.target)) return;
                    checkWatchFlash(Pursuer.target);
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.SetBlanked, SendOption.Reliable, -1);
                    writer.Write(Pursuer.target.PlayerId);
                    writer.Write(Byte.MaxValue);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.setBlanked(Pursuer.target.PlayerId, Byte.MaxValue);

                    Pursuer.target = null;

                    Pursuer.blanks++;
                    pursuerButton.Timer = pursuerButton.MaxTimer;
                    SoundEffectsManager.play("pursuerBlank");
                }

            },
            () => { return Pursuer.pursuer != null && Pursuer.pursuer == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead && Pursuer.blanks < Pursuer.blanksNumber; },
            () =>
            {
                showTargetNameOnButton(Pursuer.target, pursuerButton, "BLANK");
                if (pursuerButtonBlanksText != null) pursuerButtonBlanksText.text = $"{Pursuer.blanksNumber - Pursuer.blanks}";

                return Pursuer.blanksNumber > Pursuer.blanks && CachedPlayer.LocalPlayer.PlayerControl.CanMove && Pursuer.target != null;
            },
            () => { pursuerButton.Timer = pursuerButton.MaxTimer; },
            Pursuer.getTargetSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F
        );

        // Pursuer button blanks left
        pursuerButtonBlanksText = UnityEngine.Object.Instantiate(pursuerButton.actionButton.cooldownTimerText, pursuerButton.actionButton.cooldownTimerText.transform.parent);
        pursuerButtonBlanksText.text = "";
        pursuerButtonBlanksText.enableWordWrapping = false;
        pursuerButtonBlanksText.transform.localScale = Vector3.one * 0.5f;
        pursuerButtonBlanksText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);


        // Witch Spell button
        witchSpellButton = new CustomButton(
            () =>
            {
                if (Witch.currentTarget != null)
                {
                    if (checkAndDoVetKill(Witch.currentTarget)) return;
                    checkWatchFlash(Witch.currentTarget);
                    Witch.spellCastingTarget = Witch.currentTarget;
                    SoundEffectsManager.play("witchSpell");
                }
            },
            () => { return Witch.witch != null && Witch.witch == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () =>
            {
                showTargetNameOnButton(Witch.currentTarget, witchSpellButton, "");
                if (witchSpellButton.isEffectActive && Witch.spellCastingTarget != Witch.currentTarget)
                {
                    Witch.spellCastingTarget = null;
                    witchSpellButton.Timer = 0f;
                    witchSpellButton.isEffectActive = false;
                }
                return CachedPlayer.LocalPlayer.PlayerControl.CanMove && Witch.currentTarget != null;
            },
            () =>
            {
                showTargetNameOnButton(null, arsonistButton, "SPELL");
                witchSpellButton.Timer = witchSpellButton.MaxTimer;
                witchSpellButton.isEffectActive = false;
                Witch.spellCastingTarget = null;
            },
            Witch.getButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.F,
            true,
            Witch.spellCastingDuration,
            () =>
            {
                if (Witch.spellCastingTarget == null) return;
                MurderAttemptResult attempt = checkMuderAttempt(Witch.witch, Witch.spellCastingTarget);
                if (attempt == MurderAttemptResult.PerformKill)
                {
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.SetFutureSpelled, SendOption.Reliable, -1);
                    writer.Write(Witch.currentTarget.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.setFutureSpelled(Witch.currentTarget.PlayerId);
                }
                if (attempt == MurderAttemptResult.BlankKill || attempt == MurderAttemptResult.PerformKill)
                {
                    Witch.currentCooldownAddition += Witch.cooldownAddition;
                    witchSpellButton.MaxTimer = Witch.cooldown + Witch.currentCooldownAddition;
                    PlayerControlFixedUpdatePatch.miniCooldownUpdate();  // Modifies the MaxTimer if the witch is the mini
                    witchSpellButton.Timer = witchSpellButton.MaxTimer;
                    if (Witch.triggerBothCooldowns)
                    {
                        float multiplier = (Mini.mini != null && CachedPlayer.LocalPlayer.PlayerControl == Mini.mini) ? (Mini.isGrownUp() ? 0.66f : 2f) : 1f;
                        Witch.witch.killTimer = GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown * multiplier;
                    }
                }
                else
                {
                    witchSpellButton.Timer = 0f;
                }
                Witch.spellCastingTarget = null;
            }
        );

        // Juggernaut Kill
        juggernautKillButton = new CustomButton(
            () =>
            {
                if (checkAndDoVetKill(Juggernaut.currentTarget)) return;
                if (checkMurderAttemptAndKill(Juggernaut.juggernaut, Juggernaut.currentTarget) ==
                    MurderAttemptResult.SuppressKill) return;
                if (juggernautKillButton.MaxTimer >= 0f)
                {
                    Juggernaut.setkill();
                    juggernautKillButton.MaxTimer = Juggernaut.cooldown;
                }

                juggernautKillButton.Timer = juggernautKillButton.MaxTimer;
                Juggernaut.currentTarget = null;
            },
            () =>
            {
                return Juggernaut.juggernaut != null &&
                       Juggernaut.juggernaut == CachedPlayer.LocalPlayer.PlayerControl &&
                       !CachedPlayer.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                showTargetNameOnButton(Juggernaut.currentTarget, juggernautKillButton, "KILL");
                return Juggernaut.currentTarget && CachedPlayer.LocalPlayer.PlayerControl.CanMove;
            },
            () => { juggernautKillButton.Timer = juggernautKillButton.MaxTimer; },
            __instance.KillButton.graphic.sprite,
            new Vector3(0, 1f, 0),
            __instance,
            KeyCode.Q
        );

        // Jumper Jump
        jumperButton = new CustomButton(
            () =>
            {

                if (Jumper.jumpLocation == Vector3.zero)
                { //set location
                    Jumper.jumpLocation = PlayerControl.LocalPlayer.transform.localPosition;
                    jumperButton.Sprite = Jumper.getJumpButtonSprite();
                    Jumper.jumperCharges = Jumper.jumperChargesOnPlace;
                }
                else if (Jumper.jumperCharges >= 1f)
                { //teleport to location if you have one
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetPosition, SendOption.Reliable, -1);
                    writer.Write(PlayerControl.LocalPlayer.PlayerId);
                    writer.Write(Jumper.jumpLocation.x);
                    writer.Write(Jumper.jumpLocation.y);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);

                    PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(Jumper.jumpLocation);



                    Jumper.jumperCharges -= 1f;
                }
                if (Jumper.jumperCharges > 0) jumperButton.Timer = jumperButton.MaxTimer;
            },
            () => { return Jumper.jumper != null && Jumper.jumper == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
            () =>
            {
                //   if (jumperChargesText != null) jumperChargesText.text = $"{Jumper.jumperCharges}";
                Jumper.usedPlace = true;
                return (Jumper.jumpLocation == Vector3.zero || Jumper.jumperCharges >= 1f) && PlayerControl.LocalPlayer.CanMove;
            },
            () =>
            {
                if (Jumper.resetPlaceAfterMeeting) Jumper.resetPlaces();
                { jumperButton.Sprite = Jumper.getJumpMarkButtonSprite(); }
                //    Jumper.jumperCharges += Jumper.jumperChargesGainOnMeeting;
                //if (Jumper.jumperCharges > Jumper.jumperMaxCharges) Jumper.jumperCharges = Jumper.jumperMaxCharges;

                if (Jumper.jumperCharges > 0) jumperButton.Timer = jumperButton.MaxTimer;

            },
            Jumper.getJumpMarkButtonSprite(),
            CustomButton.ButtonPositions.lowerRowRight, //brb
            __instance,
            KeyCode.F
        );

        // Escapist Escape
        escapistButton = new CustomButton(
            () =>
            {

                if (Escapist.escapeLocation == Vector3.zero)
                { //set location
                    Escapist.escapeLocation = PlayerControl.LocalPlayer.transform.localPosition;
                    escapistButton.Sprite = Escapist.getEscapeButtonSprite();
                    Escapist.escapistCharges = Escapist.escapistChargesOnPlace;
                }
                else if (Escapist.escapistCharges >= 1f)
                { //teleport to location if you have one
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetPositionESC, SendOption.Reliable, -1);
                    writer.Write(PlayerControl.LocalPlayer.PlayerId);
                    writer.Write(Escapist.escapeLocation.x);
                    writer.Write(Escapist.escapeLocation.y);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);

                    PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(Escapist.escapeLocation);



                    Escapist.escapistCharges -= 1f;
                }
                if (Escapist.escapistCharges > 0) escapistButton.Timer = escapistButton.MaxTimer;
            },
            () => { return Escapist.escapist != null && Escapist.escapist == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
            () =>
            {
                //   if (jumperChargesText != null) jumperChargesText.text = $"{Jumper.jumperCharges}";
                Escapist.usedPlace = true;
                return (Escapist.escapeLocation == Vector3.zero || Escapist.escapistCharges >= 1f) && PlayerControl.LocalPlayer.CanMove;
            },
            () =>
            {
                if (Escapist.resetPlaceAfterMeeting) Escapist.resetPlaces();
                { escapistButton.Sprite = Escapist.getEscapeMarkButtonSprite(); }
                //    Jumper.jumperCharges += Jumper.jumperChargesGainOnMeeting;
                //if (Escapist.escapistCharges > Escapist.escapistMaxCharges) Escapist.escapistCharges = Escapist.escapistMaxCharges;

                if (Escapist.escapistCharges > 0) escapistButton.Timer = escapistButton.MaxTimer;

            },
            Escapist.getEscapeMarkButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft, //brb
            __instance,
            KeyCode.F
        );

        // Ninja mark and assassinate button 
        ninjaButton = new CustomButton(
            () =>
            {
                MessageWriter writer;
                if (Ninja.ninjaMarked != null)
                {
                    // Murder attempt with teleport
                    MurderAttemptResult attempt = checkMuderAttempt(Ninja.ninja, Ninja.ninjaMarked);
                    if (attempt == MurderAttemptResult.BodyGuardKill)
                    {
                        checkMuderAttemptAndKill(Ninja.ninja, Ninja.ninjaMarked);
                        return;
                    }
                    if (attempt == MurderAttemptResult.PerformKill || attempt == MurderAttemptResult.ReverseKill)
                    {
                        // Create first trace before killing
                        var pos = CachedPlayer.LocalPlayer.transform.position;
                        byte[] buff = new byte[sizeof(float) * 2];
                        Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                        Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                        writer = AmongUsClient.Instance.StartRpc(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.PlaceNinjaTrace, SendOption.Reliable);
                        writer.WriteBytesAndSize(buff);
                        writer.EndMessage();
                        RPCProcedure.placeNinjaTrace(buff);

                        MessageWriter invisibleWriter = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.SetInvisible, SendOption.Reliable, -1);
                        invisibleWriter.Write(Ninja.ninja.PlayerId);
                        invisibleWriter.Write(byte.MinValue);
                        AmongUsClient.Instance.FinishRpcImmediately(invisibleWriter);
                        RPCProcedure.setInvisible(Ninja.ninja.PlayerId, byte.MinValue);
                        if (!checkAndDoVetKill(Ninja.ninjaMarked))
                        {

                            // Perform Kill
                            MessageWriter writer2 = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.UncheckedMurderPlayer, SendOption.Reliable, -1);
                            writer2.Write(CachedPlayer.LocalPlayer.PlayerId);
                            writer2.Write(Ninja.ninjaMarked.PlayerId);
                            writer2.Write(byte.MaxValue);
                            AmongUsClient.Instance.FinishRpcImmediately(writer2);
                            if (SubmergedCompatibility.IsSubmerged)
                            {
                                SubmergedCompatibility.ChangeFloor(Ninja.ninjaMarked.transform.localPosition.y > -7);
                            }
                            RPCProcedure.uncheckedMurderPlayer(CachedPlayer.LocalPlayer.PlayerId, Ninja.ninjaMarked.PlayerId, byte.MaxValue);
                        }
                        // Create Second trace after killing
                        pos = Ninja.ninjaMarked.transform.position;
                        buff = new byte[sizeof(float) * 2];
                        Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                        Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                        MessageWriter writer3 = AmongUsClient.Instance.StartRpc(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.PlaceNinjaTrace, SendOption.Reliable);
                        writer3.WriteBytesAndSize(buff);
                        writer3.EndMessage();
                        RPCProcedure.placeNinjaTrace(buff);
                    }

                    if (attempt == MurderAttemptResult.BlankKill || attempt == MurderAttemptResult.PerformKill)
                    {
                        ninjaButton.Timer = ninjaButton.MaxTimer;
                        Ninja.ninja.killTimer = GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown;
                    }
                    else if (attempt == MurderAttemptResult.SuppressKill)
                    {
                        ninjaButton.Timer = 0f;
                    }
                    Ninja.ninjaMarked = null;
                    return;
                }
                if (Ninja.currentTarget != null)
                {
                    if (checkAndDoVetKill(Ninja.currentTarget)) return;
                    checkWatchFlash(Witch.currentTarget);
                    Ninja.ninjaMarked = Ninja.currentTarget;
                    ninjaButton.Timer = 5f;
                    SoundEffectsManager.play("warlockCurse");

                    // Ghost Info
                    writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ShareGhostInfo, SendOption.Reliable, -1);
                    writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                    writer.Write((byte)RPCProcedure.GhostInfoTypes.NinjaMarked);
                    writer.Write(Ninja.ninjaMarked.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                }
            },
            () => { return Ninja.ninja != null && Ninja.ninja == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () =>
            {  // CouldUse
                showTargetNameOnButton(Ninja.currentTarget, ninjaButton, "NINJA");
                ninjaButton.Sprite = Ninja.ninjaMarked != null ? Ninja.getKillButtonSprite() : Ninja.getMarkButtonSprite();
                return (Ninja.currentTarget != null || (Ninja.ninjaMarked != null && !TransportationToolPatches.isUsingTransportation(Ninja.ninjaMarked))) && CachedPlayer.LocalPlayer.PlayerControl.CanMove;
            },
            () =>
            {  // on meeting ends
                ninjaButton.Timer = ninjaButton.MaxTimer;
                Ninja.ninjaMarked = null;
            },
            Ninja.getMarkButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.F
        );

        blackmailerButton = new CustomButton(
           () =>
           { // Action when Pressed
               if (Blackmailer.currentTarget != null)
               {
                   if (checkAndDoVetKill(Blackmailer.currentTarget)) return;
                   checkWatchFlash(Blackmailer.currentTarget);
                   MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.BlackmailPlayer, SendOption.Reliable, -1);
                   writer.Write(Blackmailer.currentTarget.PlayerId);
                   AmongUsClient.Instance.FinishRpcImmediately(writer);
                   RPCProcedure.blackmailPlayer(Blackmailer.currentTarget.PlayerId);
                   blackmailerButton.Timer = blackmailerButton.MaxTimer;
               }
           },
           () => { return Blackmailer.blackmailer != null && Blackmailer.blackmailer == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
           () =>
           { // Could Use
               var text = "BLACKMAIL";
               if (Blackmailer.blackmailed != null) text = Blackmailer.blackmailed.Data.PlayerName;
               showTargetNameOnButtonExplicit(Blackmailer.currentTarget, blackmailerButton, text); //Show target name under button if setting is true
               return Blackmailer.currentTarget != null && CachedPlayer.LocalPlayer.PlayerControl.CanMove;
           },
           () => { blackmailerButton.Timer = blackmailerButton.MaxTimer; },
           Blackmailer.getBlackmailButtonSprite(),
           CustomButton.ButtonPositions.upperRowLeft, //brb
           __instance,
           KeyCode.F,
           true,
           0f,
           () => { },
           false,
           "Blackmail"
       );

        mayorMeetingButton = new CustomButton(
           () =>
           {
               CachedPlayer.LocalPlayer.NetTransform.Halt(); // Stop current movement 
               Mayor.remoteMeetingsLeft--;
               handleVampireBiteOnBodyReport(); // Manually call Vampire handling, since the CmdReportDeadBody Prefix won't be called
               handleBomber2ExplodeOnBodyReport(); // Manually call Vampire handling, since the CmdReportDeadBody Prefix won't be called
               RPCProcedure.uncheckedCmdReportDeadBody(CachedPlayer.LocalPlayer.PlayerId, Byte.MaxValue);

               MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.UncheckedCmdReportDeadBody, SendOption.Reliable, -1);
               writer.Write(CachedPlayer.LocalPlayer.PlayerId);
               writer.Write(Byte.MaxValue);
               AmongUsClient.Instance.FinishRpcImmediately(writer);
               mayorMeetingButton.Timer = 1f;
           },
           () => { return Mayor.mayor != null && Mayor.mayor == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead && Mayor.meetingButton; },
           () =>
           {
               mayorMeetingButton.actionButton.OverrideText("Emergency (" + Mayor.remoteMeetingsLeft + ")");
               bool sabotageActive = false;
               foreach (PlayerTask task in CachedPlayer.LocalPlayer.PlayerControl.myTasks.GetFastEnumerator())
                   if (task.TaskType == TaskTypes.FixLights || task.TaskType == TaskTypes.RestoreOxy || task.TaskType == TaskTypes.ResetReactor || task.TaskType == TaskTypes.ResetSeismic || task.TaskType == TaskTypes.FixComms || task.TaskType == TaskTypes.StopCharles
                       || (SubmergedCompatibility.IsSubmerged && task.TaskType == SubmergedCompatibility.RetrieveOxygenMask))
                       sabotageActive = true;
               return !sabotageActive && CachedPlayer.LocalPlayer.PlayerControl.CanMove && (Mayor.remoteMeetingsLeft > 0);
           },
           () => { mayorMeetingButton.Timer = mayorMeetingButton.MaxTimer; },
           Mayor.getMeetingSprite(),
           CustomButton.ButtonPositions.lowerRowRight,
           __instance,
           KeyCode.F,
           true,
           0f,
           () => { },
           false,
           "Meeting"
       );

        // Jackal Sidekick Button
        cultistTurnButton = new CustomButton(
            () =>
            {
                if (checkAndDoVetKill(Cultist.currentTarget)) return;
                checkWatchFlash(Cultist.currentTarget);
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.CultistCreateImposter, SendOption.Reliable, -1);
                writer.Write(Cultist.currentTarget.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.cultistCreateImposter(Cultist.currentTarget.PlayerId);
                SoundEffectsManager.play("jackalSidekick");
            },
            () => { return Cultist.needsFollower && Cultist.cultist != null && Cultist.cultist == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () =>
            {
                showTargetNameOnButton(Cultist.currentTarget, cultistTurnButton, "Convert"); // Show now text since the button already says sidekick
                return Cultist.needsFollower && Cultist.currentTarget != null && CachedPlayer.LocalPlayer.PlayerControl.CanMove;
            },
            () => { jackalSidekickButton.Timer = jackalSidekickButton.MaxTimer; },
            Cultist.getSidekickButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft, //brb
            __instance,
            KeyCode.F
        );

        // Trapper button
        trapperButton = new CustomButton(
            () =>
            {


                var pos = CachedPlayer.LocalPlayer.transform.position;
                byte[] buff = new byte[sizeof(float) * 2];
                Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                MessageWriter writer = AmongUsClient.Instance.StartRpc(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.SetTrap, SendOption.Reliable);
                writer.WriteBytesAndSize(buff);
                writer.EndMessage();
                RPCProcedure.setTrap(buff);

                SoundEffectsManager.play("trapperTrap");
                trapperButton.Timer = trapperButton.MaxTimer;
            },
            () => { return Trapper.trapper != null && Trapper.trapper == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () =>
            {
                if (trapperChargesText != null) trapperChargesText.text = $"{Trapper.charges} / {Trapper.maxCharges}";
                return CachedPlayer.LocalPlayer.PlayerControl.CanMove && Trapper.charges > 0;
            },
            () => { trapperButton.Timer = trapperButton.MaxTimer; },
            Trapper.getButtonSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F
        );

        // Bomber button
        terroristButton = new CustomButton(
            () =>
            {
                if (checkMuderAttempt(Terrorist.terrorist, Terrorist.terrorist, ignoreMedic: true) != MurderAttemptResult.BlankKill)
                {
                    var pos = CachedPlayer.LocalPlayer.transform.position;
                    byte[] buff = new byte[sizeof(float) * 2];
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                    MessageWriter writer = AmongUsClient.Instance.StartRpc(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.PlaceBomb, SendOption.Reliable);
                    writer.WriteBytesAndSize(buff);
                    writer.EndMessage();
                    RPCProcedure.placeBomb(buff);

                    SoundEffectsManager.play("trapperTrap");
                }

                if (Terrorist.selfExplosion)
                {
                    var loacl = Terrorist.terrorist.PlayerId;
                    var writer1 = AmongUsClient.Instance.StartRpcImmediately(Terrorist.terrorist.NetId,
                        (byte)CustomRPC.UncheckedMurderPlayer, SendOption.Reliable);
                    writer1.Write(loacl);
                    writer1.Write(loacl);
                    writer1.Write(byte.MaxValue);
                    AmongUsClient.Instance.FinishRpcImmediately(writer1);
                    RPCProcedure.uncheckedMurderPlayer(loacl, loacl, byte.MaxValue);
                }

                terroristButton.Timer = terroristButton.MaxTimer;
                Terrorist.isPlanted = true;
            },
            () => { return Terrorist.terrorist != null && Terrorist.terrorist == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () => { return CachedPlayer.LocalPlayer.PlayerControl.CanMove && !Terrorist.isPlanted; },
            () => { terroristButton.Timer = terroristButton.MaxTimer; },
            Terrorist.getButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.F,
            true,
            Terrorist.destructionTime,
            () =>
            {
                terroristButton.Timer = terroristButton.MaxTimer;
                terroristButton.isEffectActive = false;
                terroristButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            }
        );

        defuseButton = new CustomButton(
            () =>
            {
                defuseButton.HasEffect = true;
            },
            () =>
            {
                if (shifterShiftButton.HasButton())
                    defuseButton.PositionOffset = new Vector3(0f, 2f, 0f);
                else
                    defuseButton.PositionOffset = new Vector3(0f, 1f, 0f);
                return Terrorist.bomb != null && Bomb.canDefuse && !CachedPlayer.LocalPlayer.Data.IsDead;
            },
            () =>
            {
                if (defuseButton.isEffectActive && !Bomb.canDefuse)
                {
                    defuseButton.Timer = 0f;
                    defuseButton.isEffectActive = false;
                }
                return CachedPlayer.LocalPlayer.PlayerControl.CanMove;
            },
            () =>
            {
                defuseButton.Timer = 0f;
                defuseButton.isEffectActive = false;
            },
            Bomb.getDefuseSprite(),
            new Vector3(0f, 1f, 0),
            __instance,
            null,
            true,
            Terrorist.defuseDuration,
            () =>
            {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.DefuseBomb, SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.defuseBomb();

                defuseButton.Timer = 0f;
                Bomb.canDefuse = false;
            },
            true
        );

        thiefKillButton = new CustomButton(
            () =>
            {
                PlayerControl thief = Thief.thief;
                PlayerControl target = Thief.currentTarget;
                var result = checkMuderAttempt(thief, target);
                if (result == MurderAttemptResult.BlankKill)
                {
                    thiefKillButton.Timer = thiefKillButton.MaxTimer;
                    return;
                }

                if (Thief.suicideFlag)
                {
                    // Suicide
                    MessageWriter writer2 = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.UncheckedMurderPlayer, SendOption.Reliable, -1);
                    writer2.Write(thief.PlayerId);
                    writer2.Write(thief.PlayerId);
                    writer2.Write(0);
                    RPCProcedure.uncheckedMurderPlayer(thief.PlayerId, thief.PlayerId, 0);
                    AmongUsClient.Instance.FinishRpcImmediately(writer2);
                    Thief.thief.clearAllTasks();
                }

                // Steal role if survived.
                if (!Thief.thief.Data.IsDead && result == MurderAttemptResult.PerformKill)
                {
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ThiefStealsRole, SendOption.Reliable, -1);
                    writer.Write(target.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.thiefStealsRole(target.PlayerId);
                }
                // Kill the victim (after becoming their role - so that no win is triggered for other teams)
                if (result == MurderAttemptResult.PerformKill)
                {
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.UncheckedMurderPlayer, SendOption.Reliable, -1);
                    writer.Write(thief.PlayerId);
                    writer.Write(target.PlayerId);
                    writer.Write(byte.MaxValue);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.uncheckedMurderPlayer(thief.PlayerId, target.PlayerId, byte.MaxValue);
                }
            },
           () => { return Thief.thief != null && CachedPlayer.LocalPlayer.PlayerControl == Thief.thief && !CachedPlayer.LocalPlayer.Data.IsDead; },
           () => { return Thief.currentTarget != null && CachedPlayer.LocalPlayer.PlayerControl.CanMove; },
           () => { thiefKillButton.Timer = thiefKillButton.MaxTimer; },
           __instance.KillButton.graphic.sprite,
           CustomButton.ButtonPositions.upperRowRight,
           __instance,
           KeyCode.Q
           );

        // Trapper Charges
        trapperChargesText = UnityEngine.Object.Instantiate(trapperButton.actionButton.cooldownTimerText, trapperButton.actionButton.cooldownTimerText.transform.parent);
        trapperChargesText.text = "";
        trapperChargesText.enableWordWrapping = false;
        trapperChargesText.transform.localScale = Vector3.one * 0.5f;
        trapperChargesText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

        zoomOutButton = new CustomButton(
            () =>
            {
                toggleZoom();
            },
            () =>
            {
                if (CachedPlayer.LocalPlayer.PlayerControl == null || !CachedPlayer.LocalPlayer.Data.IsDead || (CachedPlayer.LocalPlayer.Data.Role.IsImpostor && !CustomOptionHolder.deadImpsBlockSabotage.getBool())) return false;
                var (playerCompleted, playerTotal) = TasksHandler.taskInfo(CachedPlayer.LocalPlayer.Data);
                int numberOfLeftTasks = playerTotal - playerCompleted;
                return numberOfLeftTasks <= 0 || !CustomOptionHolder.finishTasksBeforeHauntingOrZoomingOut.getBool();
            },
            () => { return true; },
            () => { return; },
            null, // Invisible button!
            new Vector3(0.4f, 2.8f, 0),
            __instance,
            KeyCode.KeypadPlus
            );
        zoomOutButton.Timer = 0f;
        /*
                    changeChatButton = new CustomButton(delegate
                {
                    if (CachedPlayer.LocalPlayer.PlayerControl == Cultist.cultist)
                    {
                        Cultist.chatTarget = Helpers.flipBitwise(Cultist.chatTarget);
                    }
                    if (CachedPlayer.LocalPlayer.PlayerControl == Follower.follower)
                    {
                        Follower.chatTarget = Helpers.flipBitwise(Follower.chatTarget);
                    }
                }, () => Helpers.isPlayerLover(CachedPlayer.LocalPlayer.PlayerControl) && Helpers.isTeamCultist(CachedPlayer.LocalPlayer.PlayerControl) && Follower.follower != null, delegate
                {
                    if (CachedPlayer.LocalPlayer.PlayerControl == Cultist.cultist)
                    {
                        //changeChatButton.Sprite = ((!Cultist.chatTarget) ? Helpers.getTeamCultistChatButtonSprite() : Helpers.getLoversChatButtonSprite());

                    }
                    if (CachedPlayer.LocalPlayer.PlayerControl == Follower.follower)
                    {
                        //changeChatButton.Sprite = ((!Follower.chatTarget) ? Helpers.getTeamCultistChatButtonSprite() : Helpers.getLoversChatButtonSprite());

                        if (Follower.chatTarget) {
                            changeChatButton.Sprite = Helpers.getTeamCultistChatButtonSprite();
                            Follower.chatTarget2 = false;

                        }
                        if (!Follower.chatTarget){
                            changeChatButton.Sprite = Helpers.getLoversChatButtonSprite();
                            Follower.chatTarget2 = false;

                        }
                    }
                    return true;
                }, delegate
                {
                }, Helpers.loadSpriteFromResources("TheOtherRoles.Resources.LoversChat.png", 150f), new Vector3(0.4f, 3.8f, 0f), __instance, KeyCode.KeypadMinus);
                changeChatButton.Timer = 0f;
                */

        hunterLighterButton = new CustomButton(
            () =>
            {
                Hunter.lightActive.Add(CachedPlayer.LocalPlayer.PlayerId);
                SoundEffectsManager.play("lighterLight");

                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ShareTimer, SendOption.Reliable, -1);
                writer.Write(Hunter.lightPunish);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.shareTimer(Hunter.lightPunish);
            },
            () => { return HideNSeek.isHunter() && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () => { return true; },
            () =>
            {
                hunterLighterButton.Timer = 30f;
                hunterLighterButton.isEffectActive = false;
                hunterLighterButton.actionButton.graphic.color = Palette.EnabledColor;
            },
            Hunter.getLightSprite(),
            CustomButton.ButtonPositions.upperRowFarLeft,
            __instance,
            KeyCode.F,
            true,
            Hunter.lightDuration,
            () =>
            {
                Hunter.lightActive.Remove(CachedPlayer.LocalPlayer.PlayerId);
                hunterLighterButton.Timer = hunterLighterButton.MaxTimer;
                SoundEffectsManager.play("lighterLight");
            }
        );

        hunterAdminTableButton = new CustomButton(
           () =>
           {
               if (!MapBehaviour.Instance || !MapBehaviour.Instance.isActiveAndEnabled)
               {
                   HudManager __instance = FastDestroyableSingleton<HudManager>.Instance;
                   __instance.InitMap();
                   MapBehaviour.Instance.ShowCountOverlay(allowedToMove: true, showLivePlayerPosition: true, includeDeadBodies: false);
               }

               CachedPlayer.LocalPlayer.NetTransform.Halt(); // Stop current movement 

               MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ShareTimer, SendOption.Reliable, -1);
               writer.Write(Hunter.AdminPunish);
               AmongUsClient.Instance.FinishRpcImmediately(writer);
               RPCProcedure.shareTimer(Hunter.AdminPunish);
           },
           () => { return HideNSeek.isHunter() && !CachedPlayer.LocalPlayer.Data.IsDead; },
           () => { return true; },
           () =>
           {
               hunterAdminTableButton.Timer = hunterAdminTableButton.MaxTimer;
               hunterAdminTableButton.isEffectActive = false;
               hunterAdminTableButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
           },
           Hacker.getAdminSprite(),
           CustomButton.ButtonPositions.lowerRowCenter,
           __instance,
           KeyCode.G,
           true,
           Hunter.AdminDuration,
           () =>
           {
               hunterAdminTableButton.Timer = hunterAdminTableButton.MaxTimer;
               if (MapBehaviour.Instance && MapBehaviour.Instance.isActiveAndEnabled) MapBehaviour.Instance.Close();
           },
           false,
           "ADMIN"
        );

        hunterArrowButton = new CustomButton(
            () =>
            {
                Hunter.arrowActive = true;
                SoundEffectsManager.play("trackerTrackPlayer");

                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ShareTimer, SendOption.Reliable, -1);
                writer.Write(Hunter.ArrowPunish);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.shareTimer(Hunter.ArrowPunish);
            },
            () => { return HideNSeek.isHunter() && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () => { return true; },
            () =>
            {
                hunterArrowButton.Timer = 30f;
                hunterArrowButton.isEffectActive = false;
                hunterArrowButton.actionButton.graphic.color = Palette.EnabledColor;
            },
            Hunter.getArrowSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.R,
            true,
            Hunter.ArrowDuration,
            () =>
            {
                Hunter.arrowActive = false;
                hunterArrowButton.Timer = hunterArrowButton.MaxTimer;
                SoundEffectsManager.play("trackerTrackPlayer");
            }
        );

        huntedShieldButton = new CustomButton(
            () =>
            {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.HuntedShield, SendOption.Reliable, -1);
                writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.huntedShield(CachedPlayer.LocalPlayer.PlayerId);
                SoundEffectsManager.play("timemasterShield");

                Hunted.shieldCount--;
            },
            () => { return HideNSeek.isHunted() && !CachedPlayer.LocalPlayer.Data.IsDead; },
            () =>
            {
                if (huntedShieldCountText != null) huntedShieldCountText.text = $"{Hunted.shieldCount}";
                return CachedPlayer.LocalPlayer.PlayerControl.CanMove && Hunted.shieldCount > 0;
            },
            () =>
            {
                huntedShieldButton.Timer = huntedShieldButton.MaxTimer;
                huntedShieldButton.isEffectActive = false;
                huntedShieldButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            },
            TimeMaster.getButtonSprite(),
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F,
            true,
            Hunted.shieldDuration,
            () =>
            {
                huntedShieldButton.Timer = huntedShieldButton.MaxTimer;
                SoundEffectsManager.stop("timemasterShield");

            }
        );

        huntedShieldCountText = UnityEngine.Object.Instantiate(huntedShieldButton.actionButton.cooldownTimerText, huntedShieldButton.actionButton.cooldownTimerText.transform.parent);
        huntedShieldCountText.text = "";
        huntedShieldCountText.enableWordWrapping = false;
        huntedShieldCountText.transform.localScale = Vector3.one * 0.5f;
        huntedShieldCountText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);


        propDisguiseButton = new CustomButton(
            () =>
            {
                // Prop stuff
                var player = PlayerControl.LocalPlayer;
                GameObject disguiseTarget = PropHunt.currentTarget;
                if (disguiseTarget != null)
                {
                    player.transform.localScale = disguiseTarget.transform.lossyScale;
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.SetProp, SendOption.Reliable, -1);
                    writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                    writer.Write(disguiseTarget.gameObject.name);
                    writer.Write(disguiseTarget.gameObject.transform.position.x);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.propHuntSetProp(CachedPlayer.LocalPlayer.PlayerId, disguiseTarget.gameObject.name, disguiseTarget.gameObject.transform.position.x);
                    SoundEffectsManager.play("morphlingMorph");
                    propDisguiseButton.Timer = 1f;
                }
            },
            () => { return PropHunt.isPropHuntGM && !PlayerControl.LocalPlayer.Data.Role.IsImpostor && !PlayerControl.LocalPlayer.Data.IsDead; },
            () =>
            {
                propSpriteRenderer.sprite = PropHunt.currentTarget?.GetComponent<SpriteRenderer>()?.sprite;
                if (propSpriteRenderer.sprite == null) propSpriteRenderer.sprite = PropHunt.currentTarget?.transform.GetComponentInChildren<SpriteRenderer>()?.sprite;
                if (propSpriteRenderer.sprite != null)
                    propSpriteHolder.transform.localScale *= 1 / propSpriteRenderer.bounds.size.magnitude;
                return PropHunt.currentTarget != null && PropHunt.currentTarget?.GetComponent<SpriteRenderer>()?.sprite != null;
            },
            () => { },
            null,
            CustomButton.ButtonPositions.lowerRowRight,
            __instance,
            KeyCode.F,
            buttonText: "DISGUISE"
            );
        propSpriteHolder = new GameObject("TORPropButtonPropSpritePreview");
        propSpriteRenderer = propSpriteHolder.AddComponent<SpriteRenderer>();
        propSpriteHolder.transform.SetParent(propDisguiseButton.actionButtonGameObject.transform, false);
        propSpriteHolder.transform.localPosition = new Vector3(0, 0, -2f);

        propHuntUnstuckButton = new CustomButton(

            () =>
            {
                PlayerControl.LocalPlayer.Collider.enabled = false;
            },
            () => { return PropHunt.isPropHuntGM && !PlayerControl.LocalPlayer.Data.IsDead; },
            () => { return true; },
            () => { },
            PropHunt.getUnstuckButtonSprite(),
            CustomButton.ButtonPositions.upperRowLeft,
            __instance,
            KeyCode.LeftShift,
            true,
            1f,
            () =>
            {
                PlayerControl.LocalPlayer.Collider.enabled = true;
                propHuntUnstuckButton.Timer = propHuntUnstuckButton.MaxTimer;
            },
            buttonText: "UNSTUCK"
            );

        propHuntRevealButton = new CustomButton(
            () =>
            {
                // select a random crewplayer to reveal.
                var candidates = PlayerControl.AllPlayerControls.ToArray().Where(x => !x.Data.Role.IsImpostor && !x.Data.IsDead && !PropHunt.isCurrentlyRevealed.ContainsKey(x.PlayerId)).ToList();
                var rng = new System.Random();
                PlayerControl selectedPlayer = candidates[rng.Next(candidates.Count)];
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.SetRevealed, SendOption.Reliable, -1);
                writer.Write(selectedPlayer.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.propHuntSetRevealed(selectedPlayer.PlayerId);

            },
            () => { return PropHunt.isPropHuntGM && !PlayerControl.LocalPlayer.Data.IsDead && PlayerControl.LocalPlayer.Data.Role.IsImpostor; },
            () => { return PropHunt.timer - PropHunt.revealPunish > 0; },
            () => { },
            PropHunt.getRevealButtonSprite(),
            CustomButton.ButtonPositions.upperRowFarLeft,
            __instance,
            KeyCode.R,
            true,
            5f,
            () =>
            {
                propHuntRevealButton.Timer = propHuntRevealButton.MaxTimer;
            }
            );

        propHuntInvisButton = new CustomButton(
            () =>
            {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.PropHuntSetInvis, SendOption.Reliable, -1);
                writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.propHuntSetInvis(CachedPlayer.LocalPlayer.PlayerId);
                SoundEffectsManager.play("morphlingMorph");
            },
            () => { return PropHunt.isPropHuntGM && !PlayerControl.LocalPlayer.Data.IsDead && !PlayerControl.LocalPlayer.Data.Role.IsImpostor && PropHunt.enableInvis; },
            () => { return PropHunt.currentObject.ContainsKey(PlayerControl.LocalPlayer.PlayerId); },
            () => { },
            PropHunt.getInvisButtonSprite(),
            CustomButton.ButtonPositions.upperRowFarLeft,
            __instance,
            KeyCode.I,
            true,
            5f,
            () =>
            {
                SoundEffectsManager.play("morphlingMorph");
                propHuntInvisButton.Timer = propHuntInvisButton.MaxTimer;
            },
            buttonText: "INVIS"
            );

        propHuntSpeedboostButton = new CustomButton(
            () =>
            {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.PropHuntSetSpeedboost, SendOption.Reliable, -1);
                writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.propHuntSetSpeedboost(CachedPlayer.LocalPlayer.PlayerId);
                SoundEffectsManager.play("timemasterShield");
            },
            () => { return PropHunt.isPropHuntGM && !PlayerControl.LocalPlayer.Data.IsDead && !PlayerControl.LocalPlayer.Data.Role.IsImpostor && PropHunt.enableSpeedboost; },
            () => { return true; },
            () => { },
            PropHunt.getSpeedboostButtonSprite(),
            CustomButton.ButtonPositions.lowerRowCenter,
            __instance,
            KeyCode.G,
            true,
            5f,
            () =>
            {
                SoundEffectsManager.stop("timemasterShield");
                propHuntSpeedboostButton.Timer = propHuntSpeedboostButton.MaxTimer;
            },
            buttonText: "BOOST"
            );

        propHuntAdminButton = new CustomButton(
           () =>
           {
               if (!MapBehaviour.Instance || !MapBehaviour.Instance.isActiveAndEnabled)
               {
                   HudManager __instance = FastDestroyableSingleton<HudManager>.Instance;
                   __instance.InitMap();
                   MapBehaviour.Instance.ShowCountOverlay(allowedToMove: true, showLivePlayerPosition: true, includeDeadBodies: false);
               }

               CachedPlayer.LocalPlayer.NetTransform.Halt(); // Stop current movement
           },
           () => { return PropHunt.isPropHuntGM && !PlayerControl.LocalPlayer.Data.IsDead && PlayerControl.LocalPlayer.Data.Role.IsImpostor; },
           () =>
           {
               propHuntAdminButton.PositionOffset = PlayerControl.LocalPlayer.inVent ? CustomButton.ButtonPositions.lowerRowRight : CustomButton.ButtonPositions.upperRowCenter;
               return !PlayerControl.LocalPlayer.inVent;
           },
           () =>
           {
               propHuntAdminButton.Timer = hunterAdminTableButton.MaxTimer;
               propHuntAdminButton.isEffectActive = false;
               propHuntAdminButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
           },
           Hacker.getAdminSprite(),
           CustomButton.ButtonPositions.upperRowCenter,
           __instance,
           KeyCode.G,
           true,
           PropHunt.adminDuration,
           () =>
           {
               propHuntAdminButton.Timer = propHuntAdminButton.MaxTimer;
               if (MapBehaviour.Instance && MapBehaviour.Instance.isActiveAndEnabled) MapBehaviour.Instance.Close();
           },
           false,
           "ADMIN"
        );
        propHuntFindButton = new CustomButton(
            () =>
            {
                SoundEffectsManager.play("timemasterShield");
            },
            () => { return PropHunt.isPropHuntGM && !PlayerControl.LocalPlayer.Data.IsDead && PlayerControl.LocalPlayer.Data.Role.IsImpostor; },
            () => { return true; },
            () => { },
            PropHunt.getFindButtonSprite(),
            CustomButton.ButtonPositions.lowerRowCenter,
            __instance,
            KeyCode.F,
            true,
            5f,
            () =>
            {
                SoundEffectsManager.stop("timemasterShield");
                propHuntFindButton.Timer = propHuntFindButton.MaxTimer;
                propHuntFindButton.isEffectActive = false;
            },
            buttonText: "FIND"
            );

        // Set the default (or settings from the previous game) timers / durations when spawning the buttons
        initialized = true;
        setCustomButtonCooldowns();
        deputyHandcuffedButtons = new Dictionary<byte, List<CustomButton>>();

    }
}
