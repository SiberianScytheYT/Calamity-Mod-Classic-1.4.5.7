using CalRD.Projectiles.Rogue;
using CalRD.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class JawsOfOblivion : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Jaws of Oblivion");
/*
            Tooltip.SetDefault("Throws a tight spread of six venomous reaper fangs that stick in enemies\n" +
				"Stealth strikes cause the teeth to emit a crushing shockwave on impact\n" +
				"You're gonna need a bigger boat");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 42;
            Item.damage = 195;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.knockBack = 1f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 40;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<JawsProjectile>();
            Item.shootSpeed = 25f;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float spreadAngle = MathHelper.ToRadians(2.5f);
            Vector2 direction = new Vector2(velocity.X, velocity.Y);
            Vector2 baseDirection = direction.RotatedBy(-spreadAngle * 2.5f);

            for (int i = 0; i < 6; i++)
            {
                Vector2 currentDirection = baseDirection.RotatedBy(spreadAngle * i);
                currentDirection = currentDirection.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-1f, 1f)));

                if (player.Calamity().StealthStrikeAvailable())
                {
                    int p = Projectile.NewProjectile(source, position.X, position.Y, currentDirection.X, currentDirection.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                    Main.projectile[p].Calamity().stealthStrike = true;
                }
                else
                {
                    int p = Projectile.NewProjectile(source, position.X, position.Y, currentDirection.X, currentDirection.Y, type, (int)(damage * 1.5f), 10, player.whoAmI, 0f, 0f);
                }
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<LeviathanTeeth>());
            recipe.AddIngredient(ModContent.ItemType<ReaperTooth>(), 6);
            recipe.AddIngredient(ModContent.ItemType<Lumenite>(), 15);
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 2);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
