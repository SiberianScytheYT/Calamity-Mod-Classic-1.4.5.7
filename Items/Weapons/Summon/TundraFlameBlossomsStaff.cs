using CalRD.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Summon
{
    public class TundraFlameBlossomsStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Tundra Flame Blossoms Staff");
/*
            Tooltip.SetDefault("Summons three unusual flowers over your head\n" +
			"Each flower consumes one minion slot");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 45;
            Item.mana = 10;
            Item.width = 52;
            Item.height = 60;
            Item.useTime = Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = 5;
            Item.UseSound = SoundID.Item46;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TundraFlameBlossom>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] < 3; //If you already have all 3, no need to resummon

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			CalamityUtils.KillShootProjectiles(false, type, player);
            for (int i = 0; i < 3; i++)
            {
                Projectile blossom = Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), player.Center, Vector2.Zero, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                blossom.ai[1] = (int)(MathHelper.TwoPi / 3f * i * 32f);
                blossom.rotation = blossom.ai[1] / 32f;
            }
            return false;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CinderBlossomStaff>());
            recipe.AddIngredient(ModContent.ItemType<FrostBlossomStaff>());
            recipe.AddIngredient(ItemID.HallowedBar, 5);
            recipe.AddIngredient(ItemID.SoulofLight, 15);
            recipe.AddIngredient(ItemID.SoulofNight, 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
