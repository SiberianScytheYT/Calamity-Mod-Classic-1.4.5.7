using CalRD.Items.Materials;
using CalRD.Projectiles.Melee;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class TheLastMourning : ModItem
    {
        public static int BaseDamage = 480;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Last Mourning");
/*
            Tooltip.SetDefault("Summons flaming pumpkins and mourning skulls that split into fire orbs on enemy hits");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 94;
            Item.height = 94;
			Item.scale = 1.5f;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.damage = BaseDamage;
            Item.knockBack = 8.5f;
            Item.useAnimation = 24;
            Item.useTime = 24;
            Item.autoReuse = true;
            Item.useTurn = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;

            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
            Item.value = Item.buyPrice(1, 40, 0, 0);
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            int logicCheckScreenHeight = Main.LogicCheckScreenHeight;
            int logicCheckScreenWidth = Main.LogicCheckScreenWidth;
            int num = Main.rand.Next(100, 300);
            int num2 = Main.rand.Next(100, 300);
            switch (Main.rand.Next(4))
            {
                case 0:
                    num -= logicCheckScreenWidth / 2 + num;
                    break;
                case 1:
                    num += logicCheckScreenWidth / 2 - num;
                    break;
                case 2:
                    num2 -= logicCheckScreenHeight / 2 + num2;
                    break;
                case 3:
                    num2 += logicCheckScreenHeight / 2 - num2;
                    break;
                default:
                    break;
            }
            num += (int)player.position.X;
            num2 += (int)player.position.Y;
            float speed = 8f;
            Vector2 vector = new Vector2((float)num, (float)num2);
            float num3 = target.position.X - vector.X;
            float num4 = target.position.Y - vector.Y;
            float num5 = (float)Math.Sqrt((double)(num3 * num3 + num4 * num4));
            num5 = speed / num5;
            num3 *= num5;
            num4 *= num5;
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), (float)num, (float)num2, num3, num4, ModContent.ProjectileType<MourningSkull>(), (int)(Item.damage * (player.GetDamage(DamageClass.Generic).Additive + player.GetDamage(DamageClass.Melee).Additive - 1f) * 1.5f), Item.knockBack, player.whoAmI, (float)target.whoAmI, 0f);
            CalamityGlobalItem.HorsemansBladeOnHit(player, target.whoAmI, (int)(Item.damage * (player.GetDamage(DamageClass.Generic).Additive + player.GetDamage(DamageClass.Melee).Additive - 1f) * 1.5f), Item.knockBack, true);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            int logicCheckScreenHeight = Main.LogicCheckScreenHeight;
            int logicCheckScreenWidth = Main.LogicCheckScreenWidth;
            int num = Main.rand.Next(100, 300);
            int num2 = Main.rand.Next(100, 300);
            switch (Main.rand.Next(4))
            {
                case 0:
                    num -= logicCheckScreenWidth / 2 + num;
                    break;
                case 1:
                    num += logicCheckScreenWidth / 2 - num;
                    break;
                case 2:
                    num2 -= logicCheckScreenHeight / 2 + num2;
                    break;
                case 3:
                    num2 += logicCheckScreenHeight / 2 - num2;
                    break;
                default:
                    break;
            }
            num += (int)player.position.X;
            num2 += (int)player.position.Y;
            float speed = 8f;
            Vector2 vector = new Vector2((float)num, (float)num2);
            float num3 = target.position.X - vector.X;
            float num4 = target.position.Y - vector.Y;
            float num5 = (float)Math.Sqrt((double)(num3 * num3 + num4 * num4));
            num5 = speed / num5;
            num3 *= num5;
            num4 *= num5;
            Projectile.NewProjectile(player.GetSource_ItemUse(Item), (float)num, (float)num2, num3, num4, ModContent.ProjectileType<MourningSkull>(), (int)(Item.damage * (player.GetDamage(DamageClass.Generic).Additive + player.GetDamage(DamageClass.Melee).Additive - 1f) * 1.5f), Item.knockBack, player.whoAmI, (float)target.whoAmI, 0f);
            CalamityGlobalItem.HorsemansBladeOnHit(player, target.whoAmI, (int)(Item.damage * (player.GetDamage(DamageClass.Generic).Additive + player.GetDamage(DamageClass.Melee).Additive - 1f) * 1.5f), Item.knockBack, true);
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dustType = 5;
                switch (Main.rand.Next(3))
                {
                    case 0:
                        dustType = 5;
                        break;
                    case 1:
                        dustType = 6;
                        break;
                    case 2:
                        dustType = 174;
                        break;
                    default:
                        break;
                }
                int dust = Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, dustType, (float)(player.direction * 2), 0f, 150, default, 1.3f);
                Main.dust[dust].velocity *= 0.2f;
            }
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddIngredient(ModContent.ItemType<BalefulHarvester>());
            r.AddIngredient(ItemID.SoulofNight, 30);
            r.AddIngredient(ModContent.ItemType<ReaperTooth>(), 5);
            r.AddIngredient(ModContent.ItemType<RuinousSoul>(), 3);
            r.AddTile(TileID.LunarCraftingStation);
            r.Register();
        }
    }
}
