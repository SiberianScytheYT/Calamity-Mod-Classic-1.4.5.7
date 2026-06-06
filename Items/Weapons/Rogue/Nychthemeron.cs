using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class Nychthemeron : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Nychthemeron");
/*
            Tooltip.SetDefault("Throws a spiky ball that ignores gravity and summons a pair of dark and light orbs that orbit the player\n" +
                "Once the spiky ball disappears the orbs will home in on the nearest target\n" +
                "Stacks up to 10\n" +
                "Stealth strikes cause all spiky balls and orbs to be thrown at once\n" +
				"Right click to recall all existing spiky balls");
*/
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SafeSetDefaults()
        {
            Item.width = 18;
            Item.damage = 60;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.knockBack = 1f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 18;
            Item.maxStack = 10;
            Item.value = Item.buyPrice(0, 3, 60, 0);
            Item.rare = 6;
            Item.shoot = ModContent.ProjectileType<NychthemeronProjectile>();
            Item.shootSpeed = 6f;
            Item.Calamity().rogue = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.type == ModContent.ProjectileType<NychthemeronProjectile>() && p.owner == player.whoAmI)
                {
                    p.ai[0] = 1f;
                }
            }
            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int orbDamage = (int)(damage * 0.75f);

            if (player.Calamity().StealthStrikeAvailable())
            {
                for (int j = 0; j < Item.stack - player.ownedProjectileCounts[ModContent.ProjectileType<NychthemeronProjectile>()]; j++)
                {
                    float spread = 2;
                    int pIndex = Projectile.NewProjectile(source, position.X, position.Y, velocity.X + Main.rand.NextFloat(-spread, spread), velocity.Y + Main.rand.NextFloat(-spread, spread), type, damage, Item.knockBack, player.whoAmI, 0f, 1f);
                    Projectile p = Main.projectile[pIndex];
                    p.Calamity().stealthStrike = true;
                    int pID = p.identity;

                    CreateOrbs(source, position, orbDamage, Item.knockBack, pID, player);
                }
            }
            else
            {
                int pIndex = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 1f);
                int pID = Main.projectile[pIndex].identity;
                
                CreateOrbs(source, position, orbDamage, Item.knockBack, pID, player);
            }
            return false;
        }

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.shoot = 0;
				Item.shootSpeed = 0f;
				return player.ownedProjectileCounts[ModContent.ProjectileType<NychthemeronProjectile>()] > 0;
			}
			else
			{
				Item.shoot = ModContent.ProjectileType<NychthemeronProjectile>();
				Item.shootSpeed = 6f;
				int UseMax = Item.stack;
				return player.ownedProjectileCounts[ModContent.ProjectileType<NychthemeronProjectile>()] < UseMax;
			}
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SpikyBall, 30);
            recipe.AddIngredient(ItemID.LightShard);
            recipe.AddIngredient(ItemID.DarkShard);
            recipe.AddIngredient(ItemID.HallowedBar, 2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        private static void CreateOrbs(IEntitySource source, Vector2 position, int damage, float knockBack, int projectileID, Player player)
        {
            float rotationOffset = 0f;

            // Ideally new projectiles will fill in the most recently vacated spots in the pattern
            int[] activeSlots = new int[10] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.type == ModContent.ProjectileType<NychthemeronOrb>() && proj.owner == player.whoAmI && proj.active && proj.localAI[0] == 0f && activeSlots[(int)proj.localAI[1]] == -1)
                {
                    activeSlots[(int)proj.localAI[1]] = i;
                }
            }

            int pos = 0;
            bool assignedOffset = false;
            for (int i = 0; i < 10; i++)
            {
                if (activeSlots[i] != -1)
                {
                    rotationOffset = Main.projectile[activeSlots[i]].rotation;
                    assignedOffset = true;
                }
                if (activeSlots[i] == -1 && assignedOffset)
                {
                    pos = i;
                    break;
                }
            }

            float orb1Col = 0f;
            float orb2Col = 1f;

            if (pos > 0 && pos < 5)
            {
                rotationOffset += MathHelper.ToRadians(45f);

                orb1Col = pos % 2;
                orb2Col = pos % 2;
            }
            else if (pos >= 5)
            {
                rotationOffset += MathHelper.ToRadians(72f);
            }

            int orb1 = Projectile.NewProjectile(source, position.X, position.Y, 0f, 0f, ModContent.ProjectileType<NychthemeronOrb>(), damage, knockBack, player.whoAmI, orb1Col, projectileID);
            int orb2 = Projectile.NewProjectile(source, position.X, position.Y, 0f, 0f, ModContent.ProjectileType<NychthemeronOrb>(), damage, knockBack, player.whoAmI, orb2Col, projectileID);
            Main.projectile[orb1].localAI[1] = pos;
            Main.projectile[orb2].localAI[1] = pos;
            Main.projectile[orb1].rotation = rotationOffset;
            Main.projectile[orb2].rotation = rotationOffset + MathHelper.ToRadians(180f);
        }
    }
}
