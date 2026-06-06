using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
	public class SoulPiercer : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Soul Piercer");
/*
            Tooltip.SetDefault("Casts a powerful ray that summons extra rays on enemy hits");
*/
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 155;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 19;
            Item.width = 64;
            Item.height = 64;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 8f;
            Item.value = Item.buyPrice(1, 80, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item73;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SoulPiercerBeam>();
            Item.shootSpeed = 6f;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        /*public override Vector2? HoldoutOrigin()
        {
            return new Vector2(15, 15);
        }*/

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
			Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalRD/Items/Weapons/Magic/SoulPiercerGlow").Value);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 12);
            recipe.AddRecipeGroup("NForEE", 6);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
