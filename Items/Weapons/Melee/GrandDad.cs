using CalRD.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class GrandDad : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Grand Dad");
/*
            Tooltip.SetDefault("Lowers enemy defense to 0 when they are struck\n" +
                        "Yeets enemies across space and time\n" +
                        "7");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 124;
            Item.height = 124;
            Item.damage = 777;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 25;
            Item.useTurn = true;
            Item.knockBack = 77f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(1, 0, 0, 0);
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.RareVariant;
        }

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			player.itemLocation += new Vector2(-12f * player.direction, 12f * player.gravDir).RotatedBy(player.itemRotation);
		}

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (CalamityGlobalNPC.ShouldAffectNPC(target))
            {
                target.knockBackResist = 7f;
                target.defense = 0;
            }
        }
    }
}
