using CalRD.Items.Materials;
using CalRD.Items.Weapons.Rogue;
using CalRD.Projectiles.DraedonsArsenal;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.DraedonsArsenal
{
    public class PlasmaGrenade : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Plasma Grenade");
/*
            Tooltip.SetDefault("Each grenade contains a heavily condensed and heated unit of plasma. Use with care.\n" +
                               "Throws a grenade that explodes into plasma on collision\n" +
                               "Stealth strikes explode violently on collision into a vaporizing blast\n" +
                               "Deals more damage against enemies with high defenses");
*/
        }

        public override void SafeSetDefaults()
        {
            CalamityGlobalItem modItem = Item.Calamity();

            Item.width = 22;
            Item.height = 28;
            Item.damage = 17800;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = Item.useTime = 27;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.value = Item.sellPrice(silver: 80);
            Item.rare = ItemRarityID.Red;
            modItem.customRarity = CalamityRarity.DraedonRust;

            Item.shoot = ModContent.ProjectileType<PlasmaGrenadeProjectile>();
            Item.shootSpeed = 14f;
            modItem.rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile grenade = Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI);
            grenade.Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(999);
            recipe.AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 5);
            recipe.AddIngredient(ModContent.ItemType<DubiousPlating>(), 5);
            recipe.AddIngredient(ModContent.ItemType<AuricBar>(), 1);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
