using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class MycelialClaws : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mycelial Claws");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.damage = 14;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 6;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 6;
            Item.useTurn = true;
            Item.knockBack = 3.75f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 24;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = 2;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(4))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 56);
            }
        }
    }
}
