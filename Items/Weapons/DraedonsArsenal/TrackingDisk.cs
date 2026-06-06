using CalRD.Items.Materials;
using CalRD.Items.Weapons.Rogue;
using CalRD.Projectiles.DraedonsArsenal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.DraedonsArsenal
{
    public class TrackingDisk : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Tracking Disk");
/*
            Tooltip.SetDefault("A weapon that, as it flies, processes calculations and fires lasers.\n" +
                               "Releases a flying disk that fires lasers at nearby enemies\n" +
                               "Stealth strikes allow the disk to fire multiple larger lasers at different targets.\n" +
                               "Deals less damage against enemies with high defense");
*/
        }
        public override void SafeSetDefaults()
        {
            CalamityGlobalItem modItem = Item.Calamity();

            Item.damage = 25;
            modItem.rogue = true;

            Item.width = 30;
            Item.height = 34;
            Item.useTime = 42;
            Item.useAnimation = 42;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = false;
            Item.knockBack = 3f;

            Item.value = CalamityGlobalItem.Rarity3BuyPrice;
            Item.rare = ItemRarityID.Red;
            modItem.customRarity = CalamityRarity.DraedonRust;

            Item.noUseGraphic = true;

            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.shoot = ModContent.ProjectileType<TrackingDiskProjectile>();
            Item.shootSpeed = 10f;

            modItem.UsesCharge = true;
            modItem.MaxCharge = 50f;
            modItem.ChargePerUse = 0.08f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f).Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 5);
            recipe.AddIngredient(ModContent.ItemType<DubiousPlating>(), 7);
            recipe.AddIngredient(ModContent.ItemType<AerialiteBar>(), 4);
            recipe.AddIngredient(ItemID.MeteoriteBar, 4);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
