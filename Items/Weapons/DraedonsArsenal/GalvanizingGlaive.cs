using CalRD.Items.Materials;
using CalRD.Projectiles.DraedonsArsenal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.DraedonsArsenal
{
    public class GalvanizingGlaive : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Galvanizing Glaive");
/*
            Tooltip.SetDefault("Its use as a tool is to quickly separate a single object into two. That is also its use as a weapon.\n" +
            "Swings a spear which envelops struck foes in an energy field\n" + 
            "When done swinging, the spear discharges an extra pulse of energy\n" +
            "Deals more damage against enemies with high defenses");
*/
        }

        public override void SetDefaults()
        {
            CalamityGlobalItem modItem = Item.Calamity();

            Item.width = 56;
            Item.height = 52;
            Item.damage = 100;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 21;
            Item.useTime = 21;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 9f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            
            Item.rare = ItemRarityID.Red;
            modItem.customRarity = CalamityRarity.DraedonRust;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;
            
            Item.shoot = ModContent.ProjectileType<GalvanizingGlaiveProjectile>();
            Item.shootSpeed = 21f;

            modItem.UsesCharge = true;
            modItem.MaxCharge = 135f;
            modItem.ChargePerUse = 0.075f;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float swingOffset = Main.rand.NextFloat(0.5f, 1f) * Item.shootSpeed * 1.6f * player.direction;
            Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, swingOffset);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 12);
            recipe.AddIngredient(ModContent.ItemType<DubiousPlating>(), 18);
            recipe.AddIngredient(ModContent.ItemType<BarofLife>(), 5);
            recipe.AddIngredient(ModContent.ItemType<InfectedArmorPlating>(), 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
