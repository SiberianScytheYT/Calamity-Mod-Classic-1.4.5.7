using CalRD.Items.Materials;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Summon;
using CalRD.Projectiles.Magic;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class Apotheosis : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Apotheosis");
/*
            Tooltip.SetDefault("Eat worms\n" +
                "Unleashes interdimensional projection magic");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 420;
            Item.DamageType = DamageClass.Magic;
            Item.mana = (int)42.0;
            Item.width = 30;
            Item.height = 34;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTurn = false;
            Item.noMelee = true;
            Item.knockBack = 6.9f;
            Item.value = Item.buyPrice(5, 0, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item92;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ApothMark>();
            Item.shootSpeed = 15.69f;
            Item.Calamity().customRarity = CalamityRarity.Developer;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
			Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalRD/Items/Weapons/Magic/ApotheosisGlow").Value);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SpellTome);
            recipe.AddIngredient(ModContent.ItemType<CosmicDischarge>());
            recipe.AddIngredient(ModContent.ItemType<StaffoftheMechworm>(), 2);
            recipe.AddIngredient(ModContent.ItemType<Excelsus>(), 2);
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 7);
            recipe.AddIngredient(ModContent.ItemType<DarksunFragment>(), 33);
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 33);
            recipe.AddIngredient(ModContent.ItemType<ShadowspecBar>(), 5);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}
