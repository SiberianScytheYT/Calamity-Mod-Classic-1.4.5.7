using CalRD.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Items.Weapons.Summon
{
    public class AngryChickenStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Yharon's Kindle Staff");
/*
            Tooltip.SetDefault("Summons the Son of Yharon to fight for you\n" +
                               "The dragon increases your life regen, defense, and movement speed while summoned\n" +
                               "Requires 4 minion slots to use");
*/
        }

        public override void SetDefaults()
        {
            Item.mana = 50;
            Item.damage = 130;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = Item.useAnimation = 10;
            Item.noMelee = true;
            Item.knockBack = 7f;
            Item.value = Item.buyPrice(1, 80, 0, 0);
            Item.rare = 10;
            Item.UseSound = new SoundStyle("CalRD/Sounds/Item/FlareSound");
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SonOfYharon>();
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)
            {
                position = Main.MouseWorld;
                velocity.X = 0;
                velocity.Y = 0;
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI);
            }
            return false;
        }
    }
}
