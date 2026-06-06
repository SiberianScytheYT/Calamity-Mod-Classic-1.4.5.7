using CalRD.Items.Ammo;
using CalRD.Items.Materials;
using CalRD.Projectiles.Magic;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class IceBarrage : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Ice Barrage");
/*
            Tooltip.SetDefault("Oh dear, you are dead!\n" +
							   "Casts a deadly and powerful ice spell in the location of the cursor\n" +
							   "This ice spell locks itself to the position of nearby enemies\n" +
                               "Consumes 2 Blood Runes every time it's used");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 60;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
            Item.value = Item.buyPrice(1, 80, 0, 0);
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 180;
            Item.noMelee = true;
            Item.UseSound = new SoundStyle("CalRD/Sounds/Item/IceBarrageCast");

            Item.damage = 5800;
            Item.knockBack = 6f;
            Item.useTime = 300;
            Item.useAnimation = 300;
            Item.reuseDelay = 60;
            Item.shoot = ModContent.ProjectileType<IceBarrageMain>();
            Item.shootSpeed = 2f;
            Item.useAmmo = ModContent.ItemType<BloodRune>();
        }

        public override bool CanUseItem(Player player)
        {
            return CalamityGlobalItem.HasEnoughAmmo(player, Item, 2);
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return false;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
            vector2.X = Main.mouseX + Main.screenPosition.X;
            vector2.Y = Main.mouseY + Main.screenPosition.Y;
            Projectile.NewProjectile(source, vector2, Vector2.Zero, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);

            CalamityGlobalItem.ConsumeAdditionalAmmo(player, Item, 2);

            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BlizzardStaff);
            recipe.AddIngredient(ItemID.IceRod);
            recipe.AddIngredient(ModContent.ItemType<IcicleStaff>());
            recipe.AddIngredient(ModContent.ItemType<EndothermicEnergy>(), 23);
            recipe.AddIngredient(ModContent.ItemType<CryoBar>(), 18);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation.X -= 8f * player.direction;
            player.itemRotation = player.direction * MathHelper.ToRadians(-45f);
        }
    }
}
