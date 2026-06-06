using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Magic
{
    public class ClothiersWrath : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Clothier's Wrath");
        }

        public override void SetDefaults()
        {
            Item.damage = 24;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 15;
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = 3;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.ClothiersCurse;
            Item.shootSpeed = 6f;
            Item.Calamity().customRarity = CalamityRarity.RareVariant;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numProj = 2;
            float rotation = MathHelper.ToRadians(2);
            for (int i = 0; i < numProj + 1; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numProj - 1)));
                int proj = Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[proj].Calamity().forceMagic = true;
            }
            return false;
        }
    }
}
