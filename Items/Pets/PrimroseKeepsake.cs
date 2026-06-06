using CalRD.Buffs.Pets;
using CalRD.Projectiles.Pets;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Pets
{
    public class PrimroseKeepsake : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Primrose Keepsake");
/*
            Tooltip.SetDefault("Did they just...");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 0;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.noMelee = true;
            Item.width = 30;
            Item.height = 30;
            Item.value = Item.sellPrice(1, 0, 0, 0);
            Item.shoot = ProjectileID.None; // neither kendra nor bear is the direct "shoot"
            Item.buffType = ModContent.BuffType<FurtasticDuoBuff>();
            Item.rare = 6;
            Item.UseSound = SoundID.Item44;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 15, true);
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            List<int> pets = new List<int> { ModContent.ProjectileType<Bear>(), ModContent.ProjectileType<KendraPet>() };
            foreach(int petProjID in pets)
                Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), petProjID, damage, Item.knockBack, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BearEye>());
            recipe.AddIngredient(ModContent.ItemType<RomajedaOrchid>());
            recipe.AddIngredient(ItemID.LovePotion);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}
