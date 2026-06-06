using CalRD.Buffs.Pets;
using CalRD.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Pets
{
    public class BloodyVein : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bloody Vein");
/*
            Tooltip.SetDefault("Summons an amalgamated pile of flesh");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 0;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.width = 24;
            Item.height = 48;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.UseSound = SoundID.NPCHit9;
            Item.shoot = ModContent.ProjectileType<PerforaMini>();
            Item.buffType = ModContent.BuffType<BloodBound>();
            Item.rare = 3;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }
    }
}
