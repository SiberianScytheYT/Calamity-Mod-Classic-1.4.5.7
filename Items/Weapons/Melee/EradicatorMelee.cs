using CalRD.Projectiles.Hybrid;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class EradicatorMelee : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Eradicator");
/*
            Tooltip.SetDefault("Throws a disk that fires lasers at nearby enemies");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.damage = 300;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 19;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 19;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.height = 54;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<EradicatorProjectile>();
            Item.shootSpeed = 12f;
            Item.Calamity().customRarity = CalamityRarity.PureGreen;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
			Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalRD/Items/Weapons/Melee/EradicatorMeleeGlow").Value);
        }
    }
}
