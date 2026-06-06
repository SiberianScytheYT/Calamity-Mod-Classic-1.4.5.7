using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class AlphaVirus : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Alpha Virus");
/*
            Tooltip.SetDefault("Throws a giant plague cell with a lethal aura that splits into 6 plague seekers in death\n" +
                               "Stealth strikes cause the plague cell to move slower, accumulating an aura of swirling plague seekers as it flies");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 44;
            Item.damage = 400;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 31;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 31;
            Item.knockBack = 1f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 44;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 16, 0, 0);
            Item.shoot = ModContent.ProjectileType<AlphaVirusProjectile>();
            Item.shootSpeed = 4f;
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int p = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, Item.shootSpeed, 0f);
                Main.projectile[p].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }

        
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<EpidemicShredder>());
            recipe.AddIngredient(ModContent.ItemType<UeliaceBar>(), 5);
            recipe.AddIngredient(ModContent.ItemType<BloodstoneCore>(), 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
        
    }
}
