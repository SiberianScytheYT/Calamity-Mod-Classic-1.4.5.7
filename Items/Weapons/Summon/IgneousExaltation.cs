using CalRD.Items.Materials;
using CalRD.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
    public class IgneousExaltation : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Igneous Exaltation");
/*
            Tooltip.SetDefault("Summons an orbiting blade\n" +
                               "Right click to launch all blades towards the cursor");
*/
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 34;
            Item.mana = 19;
            Item.width = 52;
            Item.height = 50;
            Item.useTime = Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4.5f;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<IgneousBlade>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float totalMinionSlots = 0f;
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].minion && Main.projectile[i].owner == player.whoAmI)
                {
                    totalMinionSlots += Main.projectile[i].minionSlots;
                }
            }
            if (player.altFunctionUse != 2 && totalMinionSlots < player.maxMinions)
            {
                position = Main.MouseWorld;
                Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, Item.knockBack, player.whoAmI);
                int swordCount = 0;
                for (int i = 0; i < Main.projectile.Length; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].type == type && Main.projectile[i].owner == player.whoAmI)
                    {
                        if ((Main.projectile[i].ModProjectile as IgneousBlade).Firing)
                            continue;
                        swordCount++;
                        for (int j = 0; j < 22; j++)
                        {
                            Dust dust = Dust.NewDustDirect(Main.projectile[i].position, Main.projectile[i].width, Main.projectile[i].height, 6);
                            dust.velocity = Vector2.UnitY * Main.rand.NextFloat(3f, 5.5f) * Main.rand.NextBool(2).ToDirectionInt();
                            dust.noGravity = true;
                        }
                    }
                }
                float angleVariance = MathHelper.TwoPi / swordCount;
                float angle = 0f;
                for (int i = 0; i < Main.projectile.Length; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].type == type && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].localAI[1] == 0f)
                    {
                        if ((Main.projectile[i].ModProjectile as IgneousBlade).Firing)
                            continue;
                        Main.projectile[i].ai[0] = angle;
                        angle += angleVariance;
                        for (int j = 0; j < 22; j++)
                        {
                            Dust dust = Dust.NewDustDirect(Main.projectile[i].position, Main.projectile[i].width, Main.projectile[i].height, 6);
                            dust.velocity = Vector2.UnitY * Main.rand.NextFloat(3f, 5.5f) * Main.rand.NextBool(2).ToDirectionInt();
                            dust.noGravity = true;
                        }
                    }
                }
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe r = CreateRecipe();
            r.AddIngredient(ModContent.ItemType<UnholyCore>(), 10);
            r.AddIngredient(ModContent.ItemType<EssenceofChaos>(), 5);
            r.AddTile(TileID.MythrilAnvil);
            r.Register();
        }
    }
}
