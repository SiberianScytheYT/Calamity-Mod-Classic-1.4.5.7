using CalRD.Dusts;
using CalRD.Items.Dyes;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Summon;
using CalRD.Projectiles.Summon;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.CalPlayer
{
    public class CalamityPlayerDrawEffects : ModPlayer
    {
        public override void HideDrawLayers(PlayerDrawSet drawInfo)
        {
            if (Player is null)
                return;
            
            if (drawInfo.drawPlayer.Calamity().andromedaState != AndromedaPlayerState.Inactive)
            {
                foreach (var layer in PlayerDrawLayerLoader.Layers)
                {
                    if (layer != PlayerDrawLayers.BackAcc)
                        layer.Hide();
                }
            }
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a,
            ref bool fullBright)
        {
            if (Player.Calamity().andromedaState != AndromedaPlayerState.Inactive)
                IbanDevRobot.DrawTheStupidFuckingRobot(ref drawInfo);
            
            CalamityPlayer calamityPlayer = Player.Calamity();

            // Dust modifications while high.
            if (calamityPlayer.trippy)
            {
                if (Main.myPlayer == Player.whoAmI)
                {
                    Rectangle screenArea = new Rectangle((int)Main.screenPosition.X - 500,
                        (int)Main.screenPosition.Y - 50, Main.screenWidth + 1000, Main.screenHeight + 100);
                    int dustDrawn = 0;
                    float maxShroomDust = Main.maxDustToDraw / 2;
                    for (int i = 0; i < Main.maxDustToDraw; i++)
                    {
                        Dust dust = Main.dust[i];
                        if (dust.active)
                        {
                            // Only draw dust near the screen, for performance reasons.
                            if (new Rectangle((int)dust.position.X, (int)dust.position.Y, 4, 4).Intersects(screenArea))
                            {
                                dust.color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0);
                                for (int j = 0; j < 4; j++)
                                {
                                    Vector2 dustDrawPosition = dust.position;
                                    Vector2 dustCenter = dustDrawPosition + new Vector2(4f);

                                    float distanceX = Math.Abs(dustCenter.X - Player.Center.X);
                                    float distanceY = Math.Abs(dustCenter.Y - Player.Center.Y);
                                    if (j == 0 || j == 2)
                                        dustDrawPosition.X = Player.Center.X + distanceX;
                                    else dustDrawPosition.X = Player.Center.X - distanceX;

                                    dustDrawPosition.X -= 4f;

                                    if (j == 0 || j == 1)
                                        dustDrawPosition.Y = Player.Center.Y + distanceY;

                                    else dustDrawPosition.Y = Player.Center.Y - distanceY;

                                    dustDrawPosition.Y -= 4f;
                                    Main.spriteBatch.Draw(TextureAssets.Dust.Value,
                                        dustDrawPosition - Main.screenPosition, dust.frame, dust.color, dust.rotation,
                                        new Vector2(4f), dust.scale, SpriteEffects.None, 0f);
                                    dustDrawn++;
                                }

                                // Break if too many dust clones have been drawn
                                if (dustDrawn > maxShroomDust)
                                    break;
                            }
                        }
                    }
                }
            }

            bool noRogueStealth = calamityPlayer.rogueStealth == 0f || Player.townNPCs > 2f ||
                                  !CalamityConfig.Instance.StealthInvisbility;
            if (calamityPlayer.rogueStealth > 0f && calamityPlayer.rogueStealthMax > 0f && Player.townNPCs < 3f &&
                CalamityConfig.Instance.StealthInvisbility)
            {
                // A translucent orchid color, the rogue class color
                float colorValue = calamityPlayer.rogueStealth / calamityPlayer.rogueStealthMax * 0.9f; //0 to 0.9
                r *= 1f - (colorValue * 0.89f); //255 to 50
                g *= 1f - colorValue; //255 to 25
                b *= 1f - (colorValue * 0.89f); //255 to 50
                a *= 1f - colorValue; //255 to 25
                Player.armorEffectDrawOutlines = false;
                Player.armorEffectDrawShadow = false;
                Player.armorEffectDrawShadowSubtle = false;
            }

            if (CalamityWorld.ironHeart && !Main.gameMenu)
            {
                Asset<Texture2D> ironHeart = ModContent.Request<Texture2D>("CalRD/ExtraTextures/IronHeart");
                TextureAssets.Heart = TextureAssets.Heart2 = ironHeart;
            }
            else
            {
                Asset<Texture2D> heart3 = ModContent.Request<Texture2D>("CalRD/ExtraTextures/Heart3");
                Asset<Texture2D> heart4 = ModContent.Request<Texture2D>("CalRD/ExtraTextures/Heart4");
                Asset<Texture2D> heart5 = ModContent.Request<Texture2D>("CalRD/ExtraTextures/Heart5");
                Asset<Texture2D> heart6 = ModContent.Request<Texture2D>("CalRD/ExtraTextures/Heart6");
                Asset<Texture2D>
                    heartOriginal = ModContent.Request<Texture2D>("CalRD/ExtraTextures/HeartOriginal"); // Life fruit
                Asset<Texture2D>
                    heartOriginal2 =
                        ModContent.Request<Texture2D>("CalRD/ExtraTextures/HeartOriginal2"); // Life crystal

                int totalFruit =
                    (calamityPlayer.mFruit ? 1 : 0) +
                    (calamityPlayer.bOrange ? 1 : 0) +
                    (calamityPlayer.eBerry ? 1 : 0) +
                    (calamityPlayer.dFruit ? 1 : 0);

                switch (totalFruit)
                {
                    default:
                        TextureAssets.Heart2 = heartOriginal;
                        break;
                    case 4:
                        TextureAssets.Heart2 = heart6;
                        break;
                    case 3:
                        TextureAssets.Heart2 = heart5;
                        break;
                    case 2:
                        TextureAssets.Heart2 = heart4;
                        break;
                    case 1:
                        TextureAssets.Heart2 = heart3;
                        break;
                }

                TextureAssets.Heart = heartOriginal2;
            }

            if (calamityPlayer.revivify)
            {
                if (Main.rand.NextBool(2) && drawInfo.shadow == 0f)
                {
                    int dust = Dust.NewDust(drawInfo.Position - new Vector2(2f), drawInfo.drawPlayer.width + 4,
                        drawInfo.drawPlayer.height + 4, 91, drawInfo.drawPlayer.velocity.X * 0.2f,
                        drawInfo.drawPlayer.velocity.Y * 0.2f, 100, default, 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity.Y -= 0.5f;
                }
            }

            if (calamityPlayer.tRegen)
            {
                if (Main.rand.NextBool(10) && drawInfo.shadow == 0f)
                {
                    int dust = Dust.NewDust(drawInfo.Position - new Vector2(2f), drawInfo.drawPlayer.width + 4,
                        drawInfo.drawPlayer.height + 4, 107, drawInfo.drawPlayer.velocity.X * 0.4f,
                        drawInfo.drawPlayer.velocity.Y * 0.4f, 100, default, 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0.75f;
                    Main.dust[dust].velocity.Y -= 0.35f;
                }

                if (noRogueStealth)
                {
                    r *= 0.025f;
                    g *= 0.15f;
                    b *= 0.035f;
                    fullBright = true;
                }
            }

            if (calamityPlayer.IBoots)
            {
                if (!drawInfo.drawPlayer.StandingStill() && !drawInfo.drawPlayer.mount.Active)
                {
                    if (Main.rand.NextBool(2) && drawInfo.shadow == 0f)
                    {
                        int dust = Dust.NewDust(drawInfo.Position - new Vector2(2f), drawInfo.drawPlayer.width + 4,
                            drawInfo.drawPlayer.height + 4, 229, drawInfo.drawPlayer.velocity.X * 0.4f,
                            drawInfo.drawPlayer.velocity.Y * 0.4f, 100, default, 1f);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].velocity *= 0.5f;
                    }

                    if (noRogueStealth)
                    {
                        r *= 0.05f;
                        g *= 0.05f;
                        b *= 0.05f;
                        fullBright = true;
                    }
                }
            }

            if (calamityPlayer.elysianFire)
            {
                if (!drawInfo.drawPlayer.StandingStill() && !drawInfo.drawPlayer.mount.Active)
                {
                    if (Main.rand.NextBool(2) && drawInfo.shadow == 0f)
                    {
                        int dust = Dust.NewDust(drawInfo.Position - new Vector2(2f), drawInfo.drawPlayer.width + 4,
                            drawInfo.drawPlayer.height + 4, 246, drawInfo.drawPlayer.velocity.X * 0.4f,
                            drawInfo.drawPlayer.velocity.Y * 0.4f, 100, default, 1f);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].velocity *= 0.5f;
                    }

                    if (noRogueStealth)
                    {
                        r *= 0.75f;
                        g *= 0.55f;
                        b *= 0f;
                        fullBright = true;
                    }
                }
            }

            if (calamityPlayer.dsSetBonus)
            {
                if (!drawInfo.drawPlayer.StandingStill() && !drawInfo.drawPlayer.mount.Active)
                {
                    if (Main.rand.NextBool(2) && drawInfo.shadow == 0f)
                    {
                        int dust = Dust.NewDust(drawInfo.Position - new Vector2(2f), drawInfo.drawPlayer.width + 4,
                            drawInfo.drawPlayer.height + 4, 27, drawInfo.drawPlayer.velocity.X * 0.4f,
                            drawInfo.drawPlayer.velocity.Y * 0.4f, 100, default, 1.5f);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].velocity *= 0.5f;
                    }

                    if (noRogueStealth)
                    {
                        r *= 0.15f;
                        g *= 0.025f;
                        b *= 0.1f;
                        fullBright = true;
                    }
                }
            }

            if (calamityPlayer.auricSet)
            {
                if (!drawInfo.drawPlayer.StandingStill() && !drawInfo.drawPlayer.mount.Active)
                {
                    if (drawInfo.shadow == 0f)
                    {
                        int dust = Dust.NewDust(drawInfo.Position - new Vector2(2f), Player.width + 4,
                            Player.height + 4, Main.rand.NextBool(2) ? 57 : 244, Player.velocity.X * 0.4f,
                            Player.velocity.Y * 0.4f, 100, default, 1.5f);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].velocity *= 0.5f;
                    }

                    if (noRogueStealth)
                    {
                        r *= 0.15f;
                        g *= 0.025f;
                        b *= 0.1f;
                        fullBright = true;
                    }
                }
            }

            if (calamityPlayer.bFlames || calamityPlayer.aFlames || calamityPlayer.rageModeActive)
            {
                if (Main.rand.NextBool(4) && drawInfo.shadow == 0f)
                {
                    int dust = Dust.NewDust(drawInfo.Position - new Vector2(2f), Player.width + 4, Player.height + 4,
                        ModContent.DustType<BrimstoneFlame>(), Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100,
                        default, 3f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                }

                if (noRogueStealth)
                {
                    r *= 0.25f;
                    g *= 0.01f;
                    b *= 0.01f;
                    fullBright = true;
                }
            }

            if (calamityPlayer.shadowflame)
            {
                if (Main.rand.Next(5) < 4 && drawInfo.shadow == 0f)
                {
                    int dust = Dust.NewDust(drawInfo.Position - new Vector2(2f), Player.width + 4, Player.height + 4,
                        27, Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100, default, 1.95f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0.75f;
                    Main.dust[dust].velocity.X = Main.dust[dust].velocity.X * 0.75f;
                    Main.dust[dust].velocity.Y = Main.dust[dust].velocity.Y - 1f;
                    if (Main.rand.NextBool(4))
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }
            }

            if (calamityPlayer.sulphurPoison)
            {
                if (Main.rand.Next(5) < 4 && drawInfo.shadow == 0f)
                {
                    int dust = Dust.NewDust(drawInfo.Position - new Vector2(2f), Player.width + 4, Player.height + 4,
                        46, Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100, default, 1.95f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0.75f;
                    Main.dust[dust].velocity.X = Main.dust[dust].velocity.X * 0.75f;
                    Main.dust[dust].velocity.Y = Main.dust[dust].velocity.Y - 1f;
                    if (Main.rand.NextBool(4))
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }

                if (noRogueStealth)
                {
                    r *= 0.65f;
                    b *= 0.75f;
                    fullBright = true;
                }
            }

            if (calamityPlayer.adrenalineModeActive)
            {
                if (Main.rand.NextBool(4) && drawInfo.shadow == 0f)
                {
                    int dust = Dust.NewDust(drawInfo.Position - new Vector2(2f), Player.width + 4, Player.height + 4,
                        206, Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100, default, 3f);
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                }

                if (noRogueStealth)
                {
                    r *= 0.01f;
                    g *= 0.15f;
                    b *= 0.1f;
                    fullBright = true;
                }
            }

            if (calamityPlayer.gsInferno)
            {
                if (Main.rand.NextBool(4) && drawInfo.shadow == 0f)
                {
                    int dust = Dust.NewDust(drawInfo.Position - new Vector2(2f), Player.width + 4, Player.height + 4,
                        173, Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100, default, 3f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                }

                if (noRogueStealth)
                {
                    r *= 0.25f;
                    g *= 0.01f;
                    b *= 0.01f;
                    fullBright = true;
                }
            }

            if (calamityPlayer.astralInfection)
            {
                if (Main.rand.NextBool(4) && drawInfo.shadow == 0f)
                {
                    int dustType = Main.rand.NextBool(2)
                        ? ModContent.DustType<AstralOrange>()
                        : ModContent.DustType<AstralBlue>();
                    int dust = Dust.NewDust(drawInfo.Position - new Vector2(2f), Player.width + 4, Player.height + 4,
                        dustType, Player.velocity.X * 0.2f, Player.velocity.Y * 0.2f, 100, default, 0.7f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.2f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    Main.dust[dust].color = new Color(255, 255, 255, 0);
                }
            }

            if (calamityPlayer.hFlames || calamityPlayer.hInferno)
            {
                if (Main.rand.NextBool(4) && drawInfo.shadow == 0f)
                {
                    int dust = Dust.NewDust(drawInfo.Position - new Vector2(2f), Player.width + 4, Player.height + 4,
                        ModContent.DustType<HolyFireDust>(), Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100,
                        default, 3f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                }

                if (noRogueStealth)
                {
                    r *= 0.25f;
                    g *= 0.25f;
                    b *= 0.1f;
                    fullBright = true;
                }
            }
            else if (calamityPlayer.eGravity)
            {
                if (Main.rand.NextBool(12) && drawInfo.shadow == 0f)
                {
                    int dust = Dust.NewDust(drawInfo.Position - new Vector2(2f), Player.width + 4, Player.height + 4,
                        (int)CalamityDusts.ProfanedFire, Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100,
                        default, 3f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                }
            }

            if (calamityPlayer.pFlames)
            {
                if (Main.rand.NextBool(4) && drawInfo.shadow == 0f)
                {
                    int dust = Dust.NewDust(drawInfo.Position - new Vector2(2f), Player.width + 4, Player.height + 4,
                        89, Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100, default, 3f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.2f;
                    Main.dust[dust].velocity.Y -= 0.15f;
                }

                if (noRogueStealth)
                {
                    r *= 0.07f;
                    g *= 0.15f;
                    b *= 0.01f;
                    fullBright = true;
                }
            }

            if (calamityPlayer.nightwither)
            {
                if (Main.rand.NextBool(4) && drawInfo.shadow == 0f)
                {
                    int dust = Dust.NewDust(drawInfo.Position - new Vector2(2f), Player.width + 4, Player.height + 4,
                        176, Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100, default, 3f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                }

                if (noRogueStealth)
                {
                    r *= 0.25f;
                    g *= 0.25f;
                    b *= 0.1f;
                    fullBright = true;
                }
            }

            if (calamityPlayer.vaporfied)
            {
                int dustType = Utils.SelectRandom(Main.rand, new int[]
                {
                    246,
                    242,
                    229,
                    226,
                    247,
                    187,
                    234
                });
                if (Main.rand.NextBool(4) && drawInfo.shadow == 0f)
                {
                    int dust = Dust.NewDust(drawInfo.Position - new Vector2(2f), Player.width + 4, Player.height + 4,
                        dustType, Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100, default, 3f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    if (Main.rand.NextBool(4))
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }

                if (noRogueStealth)
                {
                    r *= 0.25f;
                    g *= 0.25f;
                    b *= 0.1f;
                    fullBright = true;
                }
            }

            if (calamityPlayer.eFreeze || calamityPlayer.silvaStun || calamityPlayer.gState || calamityPlayer.cDepth ||
                calamityPlayer.eutrophication)
            {
                if (noRogueStealth)
                {
                    r *= 0f;
                    g *= 0.05f;
                    b *= 0.3f;
                    fullBright = true;
                }
            }

            if (calamityPlayer.draedonsHeart && !calamityPlayer.shadeRegen && !calamityPlayer.cFreeze &&
                Player.StandingStill() && Player.itemAnimation == 0)
            {
                if (noRogueStealth)
                {
                    r *= 0f;
                    g *= 0.5f;
                    b *= 0f;
                    fullBright = true;
                }
            }

            if (calamityPlayer.bBlood)
            {
                if (Main.rand.NextBool(6) && drawInfo.shadow == 0f)
                {
                    int dust = Dust.NewDust(drawInfo.Position - new Vector2(2f), Player.width + 4, Player.height + 4, 5,
                        Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100, default, 3f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                }

                if (noRogueStealth)
                {
                    r *= 0.15f;
                    g *= 0.01f;
                    b *= 0.01f;
                    fullBright = true;
                }
            }

            if (calamityPlayer.mushy || (calamityPlayer.etherealExtorter && Player.ZoneGlowshroom))
            {
                if (Main.rand.NextBool(6) && drawInfo.shadow == 0f)
                {
                    int dust = Dust.NewDust(drawInfo.Position - new Vector2(2f), Player.width + 4, Player.height + 4,
                        56, Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100, default, 2f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 0.5f;
                    Main.dust[dust].velocity.Y -= 0.1f;
                }

                if (noRogueStealth)
                {
                    r *= 0.15f;
                    g *= 0.01f;
                    b *= 0.01f;
                    fullBright = true;
                }
            }

            if (calamityPlayer.bloodfinBoost)
            {
                if (Main.rand.NextBool(6) && drawInfo.shadow == 0f)
                {
                    int dust = Dust.NewDust(drawInfo.Position - new Vector2(2f), Player.width + 4, Player.height + 4, 5,
                        Player.velocity.X * 0.4f, Player.velocity.Y * 0.4f, 100, default, 3f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                }

                if (noRogueStealth)
                {
                    r *= 0.5f;
                    g *= 0f;
                    b *= 0f;
                    fullBright = true;
                }
            }

            if ((calamityPlayer.cadence || calamityPlayer.ladHearts > 0) && !Player.loveStruck)
            {
                if (Main.rand.NextBool(5) && drawInfo.shadow == 0f)
                {
                    Vector2 velocity = Main.rand.NextVector2Unit();
                    velocity.X *= 0.66f;
                    velocity *= Main.rand.NextFloat(1f, 2f);

                    int heart = Gore.NewGore(calamityPlayer.Player.GetSource_FromThis(),
                        drawInfo.Position + new Vector2(Main.rand.Next(Player.width + 1),
                            Main.rand.Next(Player.height + 1)), velocity, 331, Main.rand.NextFloat(0.4f, 1.2f));
                    Main.gore[heart].sticky = false;
                    Main.gore[heart].velocity *= 0.4f;
                    Main.gore[heart].velocity.Y -= 0.6f;
                }
            }
        }

        #region Profaned Moonlight Colors

        public static readonly List<Color> MoonlightDyeDayColors = new List<Color>()
        {
            new Color(255, 163, 56),
            new Color(235, 30, 19),
            new Color(242, 48, 187),
        };

        public static readonly List<Color> MoonlightDyeNightColors = new List<Color>()
        {
            new Color(24, 134, 198),
            new Color(130, 40, 150),
            new Color(40, 64, 150),
        };

        public static void DetermineMoonlightDyeColors(out Color drawColor, Color dayColor, Color nightColor)
        {
            int totalTime = Main.dayTime ? 54000 : 32400;
            float transitionTime = 5400;
            float interval = Utils.GetLerpValue(0f, transitionTime, (float)Main.time, true) +
                             Utils.GetLerpValue(totalTime - transitionTime, totalTime, (float)Main.time, true);
            if (Main.dayTime)
            {
                // Dusk.
                if (Main.time >= totalTime - transitionTime)
                    drawColor = Color.Lerp(dayColor, nightColor,
                        Utils.GetLerpValue(totalTime - transitionTime, totalTime, (float)Main.time, true));
                // Dawn.
                else if (Main.time <= transitionTime)
                    drawColor = Color.Lerp(nightColor, dayColor, interval);
                else
                    drawColor = dayColor;
            }
            else drawColor = nightColor;
        }

        public static Color GetCurrentMoonlightDyeColor(float angleOffset = 0f)
        {
            float interval = (float)Math.Cos(Main.GlobalTimeWrappedHourly * 0.6f + angleOffset) * 0.5f + 0.5f;
            interval = MathHelper.Clamp(interval, 0f, 0.995f);
            Color dayColorToUse = CalamityUtils.MulticolorLerp(interval, MoonlightDyeDayColors.ToArray());
            Color nightColorToUse = CalamityUtils.MulticolorLerp(interval, MoonlightDyeNightColors.ToArray());
            DetermineMoonlightDyeColors(out Color drawColor, dayColorToUse, nightColorToUse);
            return drawColor;
        }

        #endregion

        #region Draw Layers

        public class AmidiasBubbleandIceShield : PlayerDrawLayer
        {
            public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.BackAcc);

            public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) =>
                drawInfo.drawPlayer.GetModPlayer<CalamityPlayer>().amidiasBlessing ||
                drawInfo.drawPlayer.GetModPlayer<CalamityPlayer>().sirenIce && drawInfo.shadow == 0f;

            protected override void Draw(ref PlayerDrawSet drawInfo)
            {
                CalamityPlayer modPlayer = drawInfo.drawPlayer.GetModPlayer<CalamityPlayer>();
                if (modPlayer.sirenIce)
                {
                    Texture2D texture = ModContent.Request<Texture2D>("CalRD/ExtraTextures/IceShield").Value;
                    int drawX = (int)(drawInfo.Position.X + drawInfo.drawPlayer.width / 2f - Main.screenPosition.X);
                    int drawY = (int)(drawInfo.Position.Y + drawInfo.drawPlayer.height / 2f -
                                      Main.screenPosition.Y); //4
                    DrawData data = new DrawData(texture, new Vector2(drawX, drawY), null, Color.Cyan, 0f,
                        new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
                    drawInfo.DrawDataCache.Add(data);
                }
                else if (modPlayer.amidiasBlessing)
                {
                    Texture2D texture = ModContent.Request<Texture2D>("CalRD/ExtraTextures/AmidiasBubble").Value;
                    int drawX = (int)(drawInfo.Position.X + drawInfo.drawPlayer.width / 2f - Main.screenPosition.X);
                    int drawY = (int)(drawInfo.Position.Y + drawInfo.drawPlayer.height / 2f -
                                      Main.screenPosition.Y); //4
                    DrawData data = new DrawData(texture, new Vector2(drawX, drawY), null, Color.White, 0f,
                        new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
                    drawInfo.DrawDataCache.Add(data);
                }
            }
        }

        public class Skin : PlayerDrawLayer
            {
                public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Skin);

                public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) =>
                    drawInfo.shadow != 0f || drawInfo.drawPlayer.dead;

                protected override void Draw(ref PlayerDrawSet drawInfo)
                {
                    Player drawPlayer = drawInfo.drawPlayer;
                    CalamityPlayer modPlayer = drawPlayer.Calamity();
                }
            }

            public class YanmeiKnifeTrail : PlayerDrawLayer
            {
                public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.BackAcc);

                public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
                {
                    Player drawPlayer = drawInfo.drawPlayer;
                    if (drawInfo.shadow != 0f || drawPlayer.dead)
                        return false;

                    return drawPlayer.Calamity().kamiBoost;
                }

                protected override void Draw(ref PlayerDrawSet drawInfo)
                {
                    Player drawPlayer = drawInfo.drawPlayer;
                    List<DrawData> existingDrawData = drawInfo.DrawDataCache;
                    for (int i = 0; i < drawPlayer.Calamity().KameiOldPositions.Length; i++)
                    {
                        float completionRatio = i / (float)drawPlayer.Calamity().KameiOldPositions.Length;
                        float scale = MathHelper.Lerp(1f, 0.5f, completionRatio);
                        float opacity = MathHelper.Lerp(0.25f, 0.08f, completionRatio);
                        List<DrawData> afterimages = new List<DrawData>();
                        for (int j = 0; j < existingDrawData.Count; j++)
                        {
                            var drawData = existingDrawData[j];
                            drawData.position = existingDrawData[j].position - drawPlayer.position +
                                                drawPlayer.oldPosition;
                            drawData.color = Color.Cyan * opacity;
                            drawData.color.G = (byte)(drawData.color.G * 1.6);
                            drawData.color.B = (byte)(drawData.color.B * 1.2);
                            drawData.scale = new Vector2(scale);
                            afterimages.Add(drawData);
                        }

                        drawInfo.DrawDataCache.InsertRange(0, afterimages);
                    }
                }
            }

            public class HeldItemGlowMaskLayer : PlayerDrawLayer
            {
                public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.HeldItem);

                protected override void Draw(ref PlayerDrawSet drawInfo)
                {
                    Player drawPlayer = drawInfo.drawPlayer;
                    List<DrawData> existingDrawData = drawInfo.DrawDataCache;

                    if (drawPlayer.JustDroppedAnItem)
                        return;

                    Item currentlyHeldItem = drawInfo.heldItem;
                    int itemType = currentlyHeldItem.type;
                    float adjustedItemScale = drawPlayer.GetAdjustedItemScale(currentlyHeldItem);

                    if (itemType < ItemID.Count)
                        return;

                    if (!drawPlayer.frozen &&
                        currentlyHeldItem.type > ItemID.None &&
                        !drawPlayer.dead &&
                        !currentlyHeldItem.noUseGraphic &&
                        (!drawPlayer.wet || !currentlyHeldItem.noWet))
                    {
                        SpriteEffects drawEffects;

                        if (drawPlayer.direction == 1)
                            drawEffects = SpriteEffects.None;
                        else drawEffects = SpriteEffects.FlipHorizontally;

                        if (drawPlayer.gravDir != 1f)
                            drawEffects |= SpriteEffects.FlipVertically;

                        if ((drawPlayer.itemAnimation > 0 && currentlyHeldItem.useStyle != 0) ||
                            (currentlyHeldItem.holdStyle > 0 && !drawPlayer.pulley))
                        {
                            // Staffs.
                            if (currentlyHeldItem.type == ModContent.ItemType<DeathhailStaff>() ||
                                currentlyHeldItem.type == ModContent.ItemType<Vesuvius>() ||
                                currentlyHeldItem.type == ModContent.ItemType<SoulPiercer>() ||
                                currentlyHeldItem.type == ModContent.ItemType<FatesReveal>() ||
                                (currentlyHeldItem.type == ModContent.ItemType<PrismaticBreaker>() &&
                                 currentlyHeldItem.useStyle == ItemUseStyleID.Shoot))
                            {
                                Texture2D texture = ModContent
                                    .Request<Texture2D>("CalRD/Items/Weapons/Magic/DeathhailStaffGlow").Value;
                                if (currentlyHeldItem.type == ModContent.ItemType<Vesuvius>())
                                    texture = ModContent.Request<Texture2D>("CalRD/Items/Weapons/Magic/VesuviusGlow")
                                        .Value;
                                else if (currentlyHeldItem.type == ModContent.ItemType<SoulPiercer>())
                                    texture = ModContent.Request<Texture2D>("CalRD/Items/Weapons/Magic/SoulPiercerGlow")
                                        .Value;
                                else if (currentlyHeldItem.type == ModContent.ItemType<FatesReveal>())
                                    texture = ModContent.Request<Texture2D>("CalRD/Items/Weapons/Magic/FatesRevealGlow")
                                        .Value;
                                else if (currentlyHeldItem.type == ModContent.ItemType<PrismaticBreaker>())
                                    texture = ModContent
                                        .Request<Texture2D>("CalRD/Items/Weapons/Melee/PrismaticBreakerGlow").Value;

                                float rotation = drawPlayer.itemRotation +
                                                 MathHelper.PiOver4 * (float)drawPlayer.direction;
                                int xOffset = 0;
                                Vector2 origin = new Vector2(0f,
                                    TextureAssets.Item[currentlyHeldItem.type].Value.Height);

                                if (drawPlayer.gravDir == -1f)
                                {
                                    if (drawPlayer.direction == -1)
                                    {
                                        rotation += MathHelper.PiOver2;
                                        origin = new Vector2(TextureAssets.Item[currentlyHeldItem.type].Value.Width,
                                            0f);
                                        xOffset -= TextureAssets.Item[currentlyHeldItem.type].Value.Width;
                                    }
                                    else
                                    {
                                        rotation -= MathHelper.PiOver2;
                                        origin = Vector2.Zero;
                                    }
                                }
                                else if (drawPlayer.direction == -1)
                                {
                                    origin = new Vector2(TextureAssets.Item[currentlyHeldItem.type].Value.Width,
                                        (float)TextureAssets.Item[currentlyHeldItem.type].Value.Height);
                                    xOffset -= TextureAssets.Item[currentlyHeldItem.type].Value.Width;
                                }

                                DrawData data = new DrawData(texture,
                                    new Vector2(
                                        (int)(drawPlayer.itemLocation.X - Main.screenPosition.X + origin.X + xOffset),
                                        (int)(drawPlayer.itemLocation.Y - Main.screenPosition.Y)),
                                    TextureAssets.Item[currentlyHeldItem.type].Value.Bounds,
                                    Color.White,
                                    rotation,
                                    origin,
                                    adjustedItemScale,
                                    drawEffects,
                                    0);

                                drawInfo.DrawDataCache.Add(data);
                            }

                            // Bow and Book.
                            else if (currentlyHeldItem.type == ModContent.ItemType<Deathwind>() ||
                                     currentlyHeldItem.type == ModContent.ItemType<Apotheosis>() ||
                                     currentlyHeldItem.type == ModContent.ItemType<CleansingBlaze>() ||
                                     currentlyHeldItem.type == ModContent.ItemType<SubsumingVortex>() ||
                                     currentlyHeldItem.type == ModContent.ItemType<AuroraBlazer>() ||
                                     currentlyHeldItem.type == ModContent.ItemType<Auralis>())
                            {
                                Texture2D texture = ModContent.Request<Texture2D>("CalRD/Items/Weapons/Ranged/DeathwindGlow").Value;
                                int offsetX = 10;
                                if (currentlyHeldItem.type == ModContent.ItemType<Apotheosis>())
                                {
                                    texture = ModContent.Request<Texture2D>("CalRD/Items/Weapons/Magic/ApotheosisGlow").Value;
                                    offsetX = 6;
                                }
                                else if (currentlyHeldItem.type == ModContent.ItemType<CleansingBlaze>())
                                {
                                    texture = ModContent.Request<Texture2D>("CalRD/Items/Weapons/Ranged/CleansingBlazeGlow").Value;
                                    offsetX = 37;
                                }
                                else if (currentlyHeldItem.type == ModContent.ItemType<SubsumingVortex>())
                                {
                                    texture = ModContent.Request<Texture2D>("CalRD/Items/Weapons/Magic/SubsumingVortexGlow").Value;
                                    offsetX = 9;
                                }
                                else if (currentlyHeldItem.type == ModContent.ItemType<AuroraBlazer>())
                                {
                                    texture = ModContent.Request<Texture2D>("CalRD/Items/Weapons/Ranged/AuroraBlazerGlow").Value;
                                    offsetX = 44;
                                }
                                else if (currentlyHeldItem.type == ModContent.ItemType<Auralis>())
                                {
                                    texture = ModContent.Request<Texture2D>("CalRD/Items/Weapons/Ranged/AuralisGlow").Value;
                                    offsetX = 62;
                                }

                                Vector2 center = TextureAssets.Item[currentlyHeldItem.type].Value.Size() * 0.5f;
                                int originOffsetX = (int)center.X - offsetX;

                                Vector2 origin = new Vector2(-originOffsetX,
                                    (float)(TextureAssets.Item[currentlyHeldItem.type].Value.Height / 2));
                                if (drawPlayer.direction == -1)
                                    origin = new Vector2(
                                        (float)(TextureAssets.Item[currentlyHeldItem.type].Value.Width + originOffsetX),
                                        (float)(TextureAssets.Item[currentlyHeldItem.type].Value.Height / 2));

                                DrawData data = new DrawData(texture,
                                    new Vector2((int)(drawPlayer.itemLocation.X - Main.screenPosition.X + center.X),
                                        (int)(drawPlayer.itemLocation.Y - Main.screenPosition.Y + center.Y)) -
                                    new Vector2((float)texture.Width * 0.5f, 0f),
                                    TextureAssets.Item[currentlyHeldItem.type].Value.Bounds,
                                    Color.White,
                                    drawPlayer.itemRotation,
                                    origin,
                                    adjustedItemScale,
                                    drawEffects,
                                    0);

                                drawInfo.DrawDataCache.Add(data);
                            }

                            // Sword.
                            else if (currentlyHeldItem.type == ModContent.ItemType<Excelsus>() ||
                                     currentlyHeldItem.type == ModContent.ItemType<EssenceFlayer>() ||
                                     currentlyHeldItem.type == ModContent.ItemType<TheEnforcer>() ||
                                     currentlyHeldItem.type == ModContent.ItemType<ElementalExcalibur>() ||
                                     currentlyHeldItem.type == ModContent.ItemType<TerrorBlade>() ||
                                     currentlyHeldItem.type == ModContent.ItemType<EtherealSubjugator>() ||
                                     (currentlyHeldItem.type == ModContent.ItemType<PrismaticBreaker>() &&
                                      currentlyHeldItem.useStyle == ItemUseStyleID.Swing))
                            {
                                Texture2D texture = ModContent
                                    .Request<Texture2D>("CalRD/Items/Weapons/Melee/ExcelsusGlow").Value;
                                if (currentlyHeldItem.type == ModContent.ItemType<EssenceFlayer>())
                                    texture = ModContent
                                        .Request<Texture2D>("CalRD/Items/Weapons/Melee/EssenceFlayerGlow").Value;
                                else if (currentlyHeldItem.type == ModContent.ItemType<TheEnforcer>())
                                    texture = ModContent.Request<Texture2D>("CalRD/Items/Weapons/Melee/TheEnforcerGlow")
                                        .Value;
                                else if (currentlyHeldItem.type == ModContent.ItemType<ElementalExcalibur>())
                                    texture = ModContent
                                        .Request<Texture2D>("CalRD/Items/Weapons/Melee/ElementalExcaliburGlow").Value;
                                else if (currentlyHeldItem.type == ModContent.ItemType<TerrorBlade>())
                                    texture = ModContent.Request<Texture2D>("CalRD/Items/Weapons/Melee/TerrorBladeGlow")
                                        .Value;
                                else if (currentlyHeldItem.type == ModContent.ItemType<EtherealSubjugator>())
                                    texture = ModContent
                                        .Request<Texture2D>("CalRD/Items/Weapons/Summon/EtherealSubjugatorGlow").Value;
                                else if (currentlyHeldItem.type == ModContent.ItemType<PrismaticBreaker>())
                                    texture = ModContent
                                        .Request<Texture2D>("CalRD/Items/Weapons/Melee/PrismaticBreakerGlow").Value;

                                float yOffset = drawPlayer.gravDir == -1f
                                    ? 0f
                                    : (float)TextureAssets.Item[currentlyHeldItem.type].Value.Height;

                                DrawData data = new DrawData(texture,
                                    new Vector2((int)(drawPlayer.itemLocation.X - Main.screenPosition.X),
                                        (int)(drawPlayer.itemLocation.Y - Main.screenPosition.Y)),
                                    TextureAssets.Item[currentlyHeldItem.type].Value.Bounds,
                                    Color.White,
                                    drawPlayer.itemRotation,
                                    new Vector2(
                                        (float)TextureAssets.Item[currentlyHeldItem.type].Value.Width * 0.5f -
                                        (float)TextureAssets.Item[currentlyHeldItem.type].Value.Width * 0.5f *
                                        drawPlayer.direction, yOffset) + Vector2.Zero,
                                    adjustedItemScale,
                                    drawEffects,
                                    0);

                                drawInfo.DrawDataCache.Add(data);
                            }
                        }
                    }
                }
            }

            public class clAfterAll : PlayerDrawLayer
                {
                    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.BackAcc);

                    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
                    {
                        Player drawPlayer = drawInfo.drawPlayer;
                        return drawPlayer.mount != null && (drawPlayer.GetModPlayer<CalamityPlayer>().fab ||
                                                            drawPlayer.GetModPlayer<CalamityPlayer>().crysthamyr ||
                                                            drawPlayer.GetModPlayer<CalamityPlayer>().onyxExcavator);
                    }

                    protected override void Draw(ref PlayerDrawSet drawInfo)
                    {
                        try
                        {
                            drawInfo.drawPlayer.mount.Draw(drawInfo.DrawDataCache, 3, drawInfo.drawPlayer,
                                drawInfo.Position,
                                drawInfo.colorMount, drawInfo.playerEffect, drawInfo.shadow);
                        }
                        catch (IndexOutOfRangeException)
                        {
                        }
                    }
                }

                public class FathomSwarmerTailLayer : PlayerDrawLayer
                {
                    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Skin);

                    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
                    {
                        Player drawPlayer = drawInfo.drawPlayer;
                        CalamityPlayer modPlayer = drawPlayer.Calamity();
                        return drawInfo.shadow == 0f && !drawPlayer.dead && modPlayer.fathomSwarmerTail;
                    }

                    protected override void Draw(ref PlayerDrawSet drawInfo)
                    {
                        Texture2D texture = ModContent
                            .Request<Texture2D>("CalRD/Items/Armor/FathomSwarmer/FathomSwarmerArmor_Tail").Value;
                        Player drawPlayer = drawInfo.drawPlayer;
                        Rectangle frame = texture.Frame(1, 4, 0, drawPlayer.Calamity().tailFrame);
                        int dyeShader = drawPlayer.dye?[2].dye ?? 0;
                        int frameSizeY = texture.Height / 4;
                        int drawX = (int)(drawInfo.Center.X - Main.screenPosition.X - (3 * drawPlayer.direction));
                        int drawY = (int)(drawInfo.Center.Y - Main.screenPosition.Y - 4f);
                        DrawData tailDrawData = new DrawData(texture, new Vector2(drawX, drawY), frame,
                            drawInfo.colorPants, 0f, new Vector2(texture.Width / 2f, frameSizeY / 2f), 1f,
                            drawInfo.playerEffect, 0)
                        {
                            shader = dyeShader
                        };
                        drawInfo.DrawDataCache.Add(tailDrawData);
                    }
                }

                public class ForbiddenSignLayer : PlayerDrawLayer
                {
                    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Skin);

                    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
                    {
                        Player drawPlayer = drawInfo.drawPlayer;
                        CalamityPlayer modPlayer = drawPlayer.Calamity();
                        return drawInfo.shadow == 0f && !drawPlayer.dead && modPlayer.forbiddenCirclet;
                    }

                    protected override void Draw(ref PlayerDrawSet drawInfo)
                    {
                        Player drawPlayer = drawInfo.drawPlayer;
                        SpriteEffects spriteEffects;
                        if (drawPlayer.direction == 1)
                            spriteEffects = SpriteEffects.None;
                        else spriteEffects = SpriteEffects.FlipHorizontally;

                        if (drawPlayer.gravDir != 1f)
                            spriteEffects |= SpriteEffects.FlipVertically;

                        int dyeShader = 0;
                        if (drawPlayer.dye[1] != null)
                            dyeShader = drawPlayer.dye[1].dye;

                        Color baseColor = drawPlayer.GetImmuneAlphaPure(
                            Lighting.GetColor((int)drawInfo.Center.X / 16, (int)drawInfo.Center.Y / 16, Color.White),
                            drawInfo.shadow);
                        Color color = Color.Lerp(baseColor, Color.White, 0.7f);
                        Texture2D texture = TextureAssets.Extra[ExtrasID.ForbiddenSign].Value;
                        Texture2D glowmask = TextureAssets.GlowMask[GlowMaskID.ForbiddenSign].Value;
                        float offsetY = (float)Math.Sin(drawPlayer.miscCounter / 300f * MathHelper.TwoPi) * 6f;
                        float sinusoidalTime = (float)Math.Cos(drawPlayer.miscCounter / 75f * MathHelper.TwoPi);
                        Color afterimageColor = new Color(80, 70, 40, 0) * (sinusoidalTime * 0.5f + 0.5f) * 0.8f;
                        float gravCheckOffset = drawPlayer.gravDir != 1f ? -20f : 20f;

                        Vector2 position =
                            new Vector2(drawInfo.Center.X - drawPlayer.bodyFrame.Width / 2 + drawPlayer.width / 2,
                                drawInfo.Center.Y + drawPlayer.height - drawPlayer.bodyFrame.Height + 4f) +
                            drawPlayer.bodyPosition;
                        position += new Vector2(drawPlayer.bodyFrame.Width / 2, drawPlayer.bodyFrame.Height / 2) +
                                    new Vector2(-drawPlayer.direction * 10, offsetY - gravCheckOffset);
                        position -= Main.screenPosition + drawPlayer.Size * 0.5f;

                        // Draw the original sign.
                        DrawData drawData = new(texture, position, null, color, drawPlayer.bodyRotation,
                            texture.Size() * 0.5f, 1f, spriteEffects, 0)
                        {
                            shader = dyeShader
                        };
                        drawInfo.DrawDataCache.Add(drawData);

                        // Draw 4 semi-transparent copies.
                        float timeX4 = sinusoidalTime * 4f;
                        Vector2 origin = texture.Size() * 0.5f;
                        for (float i = 0f; i < 4f; i++)
                        {
                            float angle = MathHelper.PiOver2 * i;
                            drawData = new(glowmask, position + angle.ToRotationVector2() * timeX4, null,
                                afterimageColor, drawPlayer.bodyRotation, origin, 1f, spriteEffects, 0);
                            drawInfo.DrawDataCache.Add(drawData);
                        }
                    }
                }

                public class ColdDivinityOverlay : PlayerDrawLayer
                {
                    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Skin);

                    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
                    {
                        Player drawPlayer = drawInfo.drawPlayer;
                        CalamityPlayer modPlayer = drawPlayer.Calamity();
                        return drawInfo.shadow == 0f && !drawPlayer.dead && modPlayer.coldDivinity;
                    }

                    protected override void Draw(ref PlayerDrawSet drawInfo)
                    {
                        Texture2D texture = ModContent
                            .Request<Texture2D>("CalRD/ExtraTextures/ColdDivinityBody").Value;
                        int drawX = (int)(drawInfo.Center.X - Main.screenPosition.X);
                        int drawY = (int)(drawInfo.Center.Y - Main.screenPosition.Y);
                        Player drawPlayer = drawInfo.drawPlayer;
                        SpriteEffects spriteEffects = drawPlayer.direction != -1
                            ? SpriteEffects.None
                            : SpriteEffects.FlipHorizontally;
                        drawInfo.DrawDataCache.Add(new DrawData(texture, new Vector2(drawX, drawY), null,
                            new Color(53, Main.DiscoG, 255) * 0.5f, 0f, texture.Size() * 0.5f, 1.15f, spriteEffects,
                            0));
                    }
                }

                public class RoverDriveShield : PlayerDrawLayer
                {
                    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Skin);

                    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
                    {
                        Player drawPlayer = drawInfo.drawPlayer;
                        CalamityPlayer modPlayer = drawPlayer.Calamity();
                        return drawInfo.shadow == 0f && !drawPlayer.dead && modPlayer.roverDriveTimer < 616 &&
                               modPlayer.roverDrive;
                    }

                    protected override void Draw(ref PlayerDrawSet drawInfo)
                    {
                        Player drawPlayer = drawInfo.drawPlayer;
                        Texture2D texture = ModContent.Request<Texture2D>("CalRD/ExtraTextures/RoverAccShield")
                            .Value;
                        Vector2 drawPos = drawPlayer.Center - Main.screenPosition + new Vector2(0f, drawPlayer.gfxOffY);
                        Rectangle frame = texture.Frame(1, 11, 0, drawPlayer.Calamity().roverFrame);
                        Color color = Color.White * 0.625f;
                        Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f / 11f);
                        float scale = 1f + (float)Math.Cos(Main.GlobalTimeWrappedHourly) * 0.1f;
                        SpriteEffects spriteEffects = drawPlayer.direction != -1
                            ? SpriteEffects.None
                            : SpriteEffects.FlipHorizontally;

                        drawInfo.DrawDataCache.Add(new DrawData(texture, drawPos, frame, color, 0f, origin, scale,
                            spriteEffects, 0));
                    }
                }

                public class StratusSphereDrawing : PlayerDrawLayer
                {
                    public override Position GetDefaultPosition() =>
                        new BeforeParent(PlayerDrawLayers.ProjectileOverArm);

                    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
                    {
                        Player drawPlayer = drawInfo.drawPlayer;
                        CalamityPlayer modPlayer = drawPlayer.Calamity();
                        return drawPlayer.inventory[drawPlayer.selectedItem].type ==
                               ModContent.ItemType<StratusSphere>();
                    }

                    protected override void Draw(ref PlayerDrawSet drawInfo)
                    {
                        if (drawInfo.drawPlayer.inventory[drawInfo.drawPlayer.selectedItem].type ==
                            ModContent.ItemType<StratusSphere>())
                        {
                            SpriteEffects effect;
                            if (drawInfo.drawPlayer.direction == 1)
                            {
                                effect = SpriteEffects.None;
                            }
                            else
                            {
                                effect = SpriteEffects.FlipHorizontally;
                            }

                            if (drawInfo.drawPlayer.gravDir != 1f)
                                effect |= SpriteEffects.FlipVertically;
                            Vector2 itemDrawPosition = drawInfo.drawPlayer.Center;
                            Texture2D drawTexture =
                                ModContent.Request<Texture2D>("CalRD/ExtraTextures/StratusSphereHold").Value;
                            Rectangle rectangle = drawTexture.Frame(1, 4, 0,
                                (int)(2 * Math.Sin(drawInfo.drawPlayer.miscCounter / 20f * MathHelper.TwoPi) + 2));
                            Vector2 drawOffset = new Vector2(rectangle.Width / 2 * drawInfo.drawPlayer.direction, 0f);
                            Vector2 origin = rectangle.Size() / 2f;
                            drawInfo.DrawDataCache.Add(new DrawData(drawTexture,
                                (itemDrawPosition - Main.screenPosition + drawOffset).Floor(),
                                new Rectangle?(rectangle),
                                Color.White,
                                drawInfo.drawPlayer.itemRotation,
                                origin,
                                drawInfo.drawPlayer.inventory[drawInfo.drawPlayer.selectedItem].scale,
                                effect,
                                0));
                            drawTexture = ModContent.Request<Texture2D>("CalRD/ExtraTextures/StratusSphereHoldGlow")
                                .Value;
                            drawInfo.DrawDataCache.Add(new DrawData(drawTexture,
                                (itemDrawPosition - Main.screenPosition + drawOffset).Floor(),
                                new Rectangle?(rectangle),
                                Color.White,
                                drawInfo.drawPlayer.itemRotation,
                                origin,
                                drawInfo.drawPlayer.inventory[drawInfo.drawPlayer.selectedItem].scale,
                                effect,
                                0));
                        }
                    }
                }
                public class ProfanedMoonlightDyeEffects : PlayerDrawLayer
                {
                    //new PlayerLayer("CalRD", "ProfanedMoonlight", PlayerLayer.Body, drawInfo =>
                    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Torso);

                    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
                    {
                        Player drawPlayer = drawInfo.drawPlayer;
                        int totalMoonlightDyes = drawPlayer.dye.Count(dyeItem =>
                            dyeItem.type == ModContent.ItemType<ProfanedMoonlightDye>());
                        return totalMoonlightDyes >= 0;
                    } 

                    protected override void Draw(ref PlayerDrawSet drawInfo)
                    {
                        Player drawPlayer = drawInfo.drawPlayer;
                        int totalMoonlightDyes = drawPlayer.dye.Count(dyeItem =>
                            dyeItem.type == ModContent.ItemType<ProfanedMoonlightDye>());
                        float auroraCount = 5 + (int)MathHelper.Clamp(totalMoonlightDyes, 0f, 4f) * 2;
                        float opacity = MathHelper.Clamp(totalMoonlightDyes / 3f, 0f, 1f);

                        opacity *= Main.dayTime ? 0.4f : 0.25f;

                        float time = Main.GlobalTimeWrappedHourly % 3f / 3f;
                        Texture2D auroraTexture =
                            ModContent.Request<Texture2D>("CalRD/ExtraTextures/AuroraTexture").Value;
                        for (int i = 0; i < auroraCount; i++)
                        {
                            float incrementOffsetAngle = MathHelper.TwoPi * i / auroraCount;
                            float xOffset = (float)Math.Sin(time * MathHelper.TwoPi + incrementOffsetAngle * 2f) * 20f;
                            float yOffset =
                                (float)Math.Sin(time * MathHelper.TwoPi + incrementOffsetAngle * 2f +
                                                MathHelper.ToRadians(60f)) *
                                6f;
                            float rotation = (float)Math.Sin(incrementOffsetAngle) * MathHelper.Pi / 12f;
                            Color color = GetCurrentMoonlightDyeColor(incrementOffsetAngle);
                            Vector2 offset = new Vector2(xOffset, yOffset - 14f);
                            DrawData drawData = new DrawData(auroraTexture,
                                drawPlayer.Top + offset - Main.screenPosition,
                                null,
                                color * opacity,
                                rotation + MathHelper.PiOver2,
                                auroraTexture.Size() * 0.5f,
                                0.135f,
                                SpriteEffects.None,
                                1);
                            drawInfo.DrawDataCache.Add(drawData);
                        }
                    }
                }
                public class AuralisAuroraEffects : PlayerDrawLayer
                {
                    //new PlayerLayer("CalRD", "AuralisAurora", PlayerLayer.Body, drawInfo =>
                    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.BackAcc);

                    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
                    {
                        if (drawInfo.shadow != 0f)
                            return false;
                        Player drawPlayer = drawInfo.drawPlayer;
                        return !(drawPlayer.Calamity().auralisAuroraCounter < 300) || drawPlayer.Calamity().auralisAuroraCooldown > 0;
                    }

                    protected override void Draw(ref PlayerDrawSet drawInfo)
                    {
                        float auroraCount = 7;
                        float opacity = 0.4f;

                        float time = Main.GlobalTimeWrappedHourly % 3f / 3f;
                        Texture2D auroraTexture =
                            ModContent.Request<Texture2D>("CalRD/ExtraTextures/AuroraTexture").Value;
                        for (int i = 0; i < auroraCount; i++)
                        {
                            float incrementOffsetAngle = MathHelper.TwoPi * i / auroraCount;
                            float xOffset = (float)Math.Sin(time * MathHelper.TwoPi + incrementOffsetAngle * 2f) * 20f;
                            float yOffset = (float)Math.Sin(time * MathHelper.TwoPi + incrementOffsetAngle * 2f + MathHelper.ToRadians(60f)) * 6f;
                            float rotation = (float)Math.Sin(incrementOffsetAngle) * MathHelper.Pi / 12f;
                            Color color = CalamityUtils.ColorSwap(Auralis.blueColor, Auralis.greenColor, 3f);
                            Vector2 offset = new Vector2(xOffset, yOffset - 14f);
                            DrawData drawData = new DrawData(auroraTexture,
                                drawInfo.drawPlayer.Top + offset - Main.screenPosition,
                                null,
                                color * opacity,
                                rotation + MathHelper.PiOver2,
                                auroraTexture.Size() * 0.5f,
                                0.135f,
                                SpriteEffects.None,
                                1);
                            drawInfo.DrawDataCache.Add(drawData);
                        }
                    }
                }

                public class IbanDevRobot : PlayerDrawLayer
                {
                    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.BackAcc);

                    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
                    {
                        if (drawInfo.shadow != 0f)
                            return false;

                        return drawInfo.drawPlayer.Calamity().andromedaState != AndromedaPlayerState.Inactive;
                    }

                    public static void DrawTheStupidFuckingRobot(ref PlayerDrawSet drawInfo)
                    {
                        Player drawPlayer = drawInfo.drawPlayer;
                        drawInfo.hidesBottomSkin = true;
                        drawInfo.hidesTopSkin = true;
                        drawInfo.armorHidesArms = true;
                        drawInfo.armorHidesHands = true;
                        drawInfo.cShield = 0;
                        drawInfo.hideCompositeShoulders = true;

                        // Clear all old draw data and draw the robot on top.
                        drawInfo.DrawDataCache.Clear();

                        int robot = -1;
                        int andromedaMechID = ModContent.ProjectileType<GiantIbanRobotOfDoom>();
                        for (int i = 0; i < Main.projectile.Length; i++)
                        {
                            if (Main.projectile[i].active && Main.projectile[i].type == andromedaMechID &&
                                Main.projectile[i].owner == drawPlayer.whoAmI)
                            {
                                robot = i;
                                break;
                            }
                        }

                        if (robot == -1)
                        {
                            drawPlayer.Calamity().andromedaState = AndromedaPlayerState.Inactive;
                            return;
                        }

                        SpriteEffects direction = Main.projectile[robot].spriteDirection == -1
                            ? SpriteEffects.FlipHorizontally
                            : SpriteEffects.None;
                        if (drawPlayer.gravDir == -1f)
                            direction |= SpriteEffects.FlipVertically;

                        GiantIbanRobotOfDoom robotEntityInstance =
                            (GiantIbanRobotOfDoom)Main.projectile[robot].ModProjectile;
                        switch (drawPlayer.Calamity().andromedaState)
                        {
                            case AndromedaPlayerState.SpecialAttack:
                                Texture2D dashTexture = ModContent
                                    .Request<Texture2D>("CalRD/ExtraTextures/AndromedaBolt").Value;
                                Rectangle frame = dashTexture.Frame(1, 4, 0,
                                    robotEntityInstance.RightIconCooldown / 4 % 4);

                                DrawData drawData = new DrawData(dashTexture,
                                    drawPlayer.Center + new Vector2(0f, drawPlayer.gravDir * -8f) - Main.screenPosition,
                                    frame,
                                    Color.White,
                                    Main.projectile[robot].rotation,
                                    drawPlayer.Size / 2,
                                    1f,
                                    direction,
                                    1);
                                drawData.shader = drawPlayer.cBody;

                                drawInfo.DrawDataCache.Add(drawData);
                                break;
                            case AndromedaPlayerState.LargeRobot:
                                Texture2D robotTexture =
                                    ModContent.Request<Texture2D>(robotEntityInstance.Texture).Value;
                                frame = new Rectangle(robotEntityInstance.FrameX * robotTexture.Width / 3,
                                    robotEntityInstance.FrameY * robotTexture.Height / 7, robotTexture.Width / 3,
                                    robotTexture.Height / 7);

                                drawData = new DrawData(
                                    ModContent.Request<Texture2D>(Main.projectile[robot].ModProjectile.Texture).Value,
                                    Main.projectile[robot].Center + Vector2.UnitY * drawPlayer.gravDir * 6f -
                                    Main.screenPosition,
                                    frame,
                                    Color.White,
                                    Main.projectile[robot].rotation,
                                    Main.projectile[robot].Size / 2,
                                    1f,
                                    direction,
                                    1);
                                drawData.shader = drawPlayer.cBody;

                                drawInfo.DrawDataCache.Add(drawData);
                                break;
                            case AndromedaPlayerState.SmallRobot:
                                robotTexture = ModContent
                                    .Request<Texture2D>("CalRD/Projectiles/Summon/AndromedaSmall").Value;
                                frame = new Rectangle(0, robotEntityInstance.CurrentFrame * 54, robotTexture.Width,
                                    robotTexture.Height / 21);
                                drawData = new DrawData(robotTexture,
                                    drawPlayer.Center +
                                    new Vector2(drawPlayer.direction == 1 ? -24 : -10, drawPlayer.gravDir * -8f) -
                                    Main.screenPosition,
                                    frame,
                                    Color.White,
                                    0f,
                                    drawPlayer.Size / 2,
                                    1f,
                                    direction,
                                    1);
                                drawData.shader = drawPlayer.cBody;

                                drawInfo.DrawDataCache.Add(drawData);
                                break;
                        }
                    }

                    protected override void Draw(ref PlayerDrawSet drawInfo)
                    {
                        DrawTheStupidFuckingRobot(ref drawInfo);
                    }
                }

                /*
                public class DyeInvisibilityFix : PlayerDrawLayer
                {
                    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.ArmOverItem);

                    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
                    {
                        Player drawPlayer = drawInfo.drawPlayer;
                        CalamityPlayer modPlayer = drawPlayer.Calamity();
                        return !drawPlayer.invis || drawPlayer.itemAnimation > 0;
                    }

                    protected override void Draw(ref PlayerDrawSet drawInfo)
                    {
                        for (int i = 0; i < drawInfo.DrawDataCache.Count; i++)
                        {
                            var copy = drawInfo.DrawDataCache[i];
                            copy.shader =
                                0; // There's no other easy solution here to my knowledge since DrawData is a value type.
                            drawInfo.DrawDataCache[i] = copy;
                        }
                    }
                }
                */
        #endregion
    }
}
        
