using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class CorrosiveSpine : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Corrosive Spine");
/*
            Tooltip.SetDefault("10% increased movement speed\n" +
                               "All rogue weapons inflict venom and spawn clouds on enemy hits\n" +
                               "You release a ton of clouds everywhere on hit");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 46;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.defense = 4;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			player.moveSpeed += 0.1f;
            player.Calamity().corrosiveSpine = true;
            if (player.immune)
            {
                if (Main.rand.NextBool(15))
                {
                    for (int i = 0; i < Main.rand.Next(3,7); i++)
                    {
                        int type = -1;
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                type = ModContent.ProjectileType<Corrocloud1>();
                                break;
                            case 1:
                                type = ModContent.ProjectileType<Corrocloud2>();
                                break;
                            case 2:
                                type = ModContent.ProjectileType<Corrocloud3>();
                                break;
                        }
                        // Should never happen, but just in case-
                        if (type != -1)
                        {
                            float speed = Main.rand.NextFloat(3f, 11f);
                            Projectile.NewProjectile(player.GetSource_Accessory(Item),player.Center, Vector2.One.RotatedByRandom(MathHelper.TwoPi) * speed,
                                type, (int)(100 * player.RogueDamage()), 0f, player.whoAmI);
                        }
                    }
                }
            }
        }
    }
}