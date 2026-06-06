using CalRD.Buffs.Mounts;
using CalRD.CalPlayer;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Mounts
{
    public class AlicornMount : ModMount
    {
        public override void SetStaticDefaults()
        {
            MountData.spawnDust = 234;
            MountData.spawnDustNoGravity = true;
            MountData.buff = ModContent.BuffType<AlicornBuff>();
            MountData.heightBoost = 34;
            MountData.fallDamage = 0f; //0.5
            MountData.runSpeed = 7f; //12
            MountData.dashSpeed = 21f; //8
            MountData.flightTimeMax = 500;
            MountData.fatigueMax = 0;
            MountData.jumpHeight = 12;
            MountData.acceleration = 0.45f;
            MountData.jumpSpeed = 8f; //10
            MountData.swimSpeed = 4f;
            MountData.blockExtraJumps = false;
            MountData.totalFrames = 15;
            MountData.constantJump = false;
            int[] array = new int[MountData.totalFrames];
            for (int l = 0; l < array.Length; l++)
            {
                array[l] = 30;
            }
            array[1] = 28;
            array[3] = 28;
            array[5] = 28;
            array[7] = 28;
            array[12] = 28;
            MountData.playerYOffsets = array;
            MountData.xOffset = 0;
            MountData.bodyFrame = 3;
            MountData.yOffset = 7; //-8
            MountData.playerHeadOffset = 36; //30
            MountData.standingFrameCount = 1;
            MountData.standingFrameDelay = 12;
            MountData.standingFrameStart = 0;
            MountData.runningFrameCount = 8; //7
            MountData.runningFrameDelay = 36; //36
            MountData.runningFrameStart = 1; //9
            MountData.flyingFrameCount = 6; //0
            MountData.flyingFrameDelay = 4; //0
            MountData.flyingFrameStart = 9; //0
            MountData.inAirFrameCount = 1; //1
            MountData.inAirFrameDelay = 12; //12
            MountData.inAirFrameStart = 10; //10
            MountData.idleFrameCount = 5; //4
            MountData.idleFrameDelay = 12; //12
            MountData.idleFrameStart = 0;
            MountData.idleFrameLoop = true;
            MountData.swimFrameCount = MountData.inAirFrameCount;
            MountData.swimFrameDelay = MountData.inAirFrameDelay;
            MountData.swimFrameStart = MountData.inAirFrameStart;
            if (Main.netMode != NetmodeID.Server)
            {
                MountData.frontTextureExtra = ModContent.Request<Texture2D>("CalRD/Items/Mounts/AlicornMountExtra");
                MountData.textureWidth = MountData.backTexture.Value.Width;
                MountData.textureHeight = MountData.backTexture.Value.Height;
            }
        }

        public override void UpdateEffects(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (modPlayer.fabsolVodka)
				player.GetDamage(DamageClass.Generic) += 0.1f;

            if (player.velocity.Length() > 9f)
            {
				int rand = Main.rand.Next(2);
                bool momo = false;
                if (rand == 1)
                {
                    momo = true;
                }
                Color meme;
                if (momo)
                {
                    meme = new Color(255, 68, 242);
                }
                else
                {
                    meme = new Color(25, 105, 255);
                }
                Rectangle rect = player.getRect();
                int dust = Dust.NewDust(new Vector2(rect.X, rect.Y), rect.Width, rect.Height, 234, 0, 0, 0, meme);
                Main.dust[dust].noGravity = true;
            }

            if (player.velocity.Y != 0f)
            {
                if (player.mount.PlayerOffset == 28)
                {
                    if (!player.flapSound)
                        SoundEngine.PlaySound(SoundID.Item32, player.position);
                    player.flapSound = true;
                }
                else
                    player.flapSound = false;
            }
        }
    }
}
