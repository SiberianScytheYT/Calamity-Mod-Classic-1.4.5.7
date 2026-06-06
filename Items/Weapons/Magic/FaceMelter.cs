using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class FaceMelter : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Face Melter");
/*
            Tooltip.SetDefault("WOOO!! FAAAAAAANTASYY WORLDDDDD!\n" +
                               "Fires music notes\n" +
                               "Right click summons an amplifier that shoots towards your mouse");
*/
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 250;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.width = 56;
            Item.height = 50;
            Item.useTime = 5;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
            Item.value = Item.buyPrice(1, 80, 0, 0);
            Item.shoot = ModContent.ProjectileType<MelterNote1>();
            Item.UseSound = SoundID.Item47;
            Item.autoReuse = true;
            Item.shootSpeed = 20f;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-15, 0);

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.TheAxe);
            recipe.AddIngredient(ItemID.MagicalHarp);
            recipe.AddIngredient(ModContent.ItemType<SirensSong>());
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 10);
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 2);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useTime = 20;
                Item.useAnimation = 20;
            }
            else
            {
                Item.useTime = 5;
                Item.useAnimation = 10;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<MelterAmp>(), damage, Item.knockBack, player.whoAmI);
                return false;
            }
            else
            {
                int note = Main.rand.Next(2);
                if (note == 0)
                {
                    damage = (int)(damage * 1.5f);
                    type = ModContent.ProjectileType<MelterNote1>();
                }
                else
                {
                    velocity.X *= 1.5f;
                    velocity.Y *= 1.5f;
                    type = ModContent.ProjectileType<MelterNote2>();
                }
                Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                return false;
            }
        }
    }
}
