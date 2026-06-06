using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CalRD.Items.Weapons.Rogue
{
    public class PhantasmalRuin : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Phantasmal Ruin");
/*
            Tooltip.SetDefault("Fires an enormous ghost lance that leaves lost souls in its wake\n"
                               +"Explodes into phantom spirits on enemy hits\n"
                               +"Stealth strikes summon ghost clones instead of lost souls");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.damage = 1500;
            Item.knockBack = 8f;

            Item.width = 102;
            Item.height = 98;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.UseSound = SoundID.Item1;
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
            Item.Calamity().rogue = true;

            Item.autoReuse = true;
            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<PhantasmalRuinProj>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable()) //setting the stealth strike
            {
                int stealth = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, (int) ((double)damage * 0.2), Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[stealth].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<LuminousStriker>());
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 4);
            recipe.AddIngredient(ModContent.ItemType<Phantoplasm>(), 20);
            recipe.AddIngredient(ModContent.ItemType <PhantomLance>(), 500);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
