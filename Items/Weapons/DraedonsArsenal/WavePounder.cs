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
    public class WavePounder : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Wave Pounder");
/*
            Tooltip.SetDefault("It utilizes its power to send heavy shockwaves throughout the area, causing agonizing internal damage.\n" +
                               "Throws a bomb which explodes into a forceful shockwave\n" +
                               "Stealth strikes emit absurdly powerful shockwaves");
*/
        }

        public override void SafeSetDefaults()
        {
            CalamityGlobalItem modItem = Item.Calamity();

            Item.damage = 102;
            modItem.rogue = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.width = 26;
            Item.height = 44;
            Item.useTime = 56;
            Item.useAnimation = 56;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 0f;

            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.rare = ItemRarityID.Red;

            modItem.customRarity = CalamityRarity.DraedonRust;
            Item.UseSound = SoundID.Item1;

            Item.shootSpeed = 16f;
            Item.shoot = ModContent.ProjectileType<WavePounderProjectile>();

            modItem.UsesCharge = true;
            modItem.MaxCharge = 190f;
            modItem.ChargePerUse = 0.5f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f).Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 12);
            recipe.AddIngredient(ModContent.ItemType<DubiousPlating>(), 18);
            recipe.AddIngredient(ModContent.ItemType<UeliaceBar>(), 8);
            recipe.AddIngredient(ItemID.LunarBar, 4);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
