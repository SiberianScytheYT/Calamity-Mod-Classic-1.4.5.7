using CalRD.Buffs.Summon;
using CalRD.CalPlayer;
using CalRD.Projectiles.Summon;
using Terraria;
using Terraria.ModLoader;
namespace CalRD.Items.Accessories
{
    public class FungalClump : ModItem
    {
		public const int FungalClumpDamage = 10;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fungal Clump");
/*
            Tooltip.SetDefault("Summons a fungal clump to fight for you\n" +
                       "The clump latches onto enemies and steals their life for you");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 26;
            Item.value = CalamityGlobalItem.Rarity2BuyPrice;
            Item.expert = true;
            Item.rare = 2;
            Item.accessory = true;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)/* tModPorter Suggestion: Consider using new hook CanAccessoryBeEquippedWith */ => !player.Calamity().fungalClump;

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.fungalClump = true;
            if (player.whoAmI == Main.myPlayer)
            {
                if (player.FindBuffIndex(ModContent.BuffType<FungalClumpBuff>()) == -1)
                {
                    player.AddBuff(ModContent.BuffType<FungalClumpBuff>(), 3600, true);
                }
                if (player.ownedProjectileCounts[ModContent.ProjectileType<FungalClumpMinion>()] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center.X, player.Center.Y, 0f, -1f, ModContent.ProjectileType<FungalClumpMinion>(), (int)(FungalClumpDamage * player.MinionDamage()), 1f, player.whoAmI, 0f, 0f);
                }
            }
        }
    }
}
