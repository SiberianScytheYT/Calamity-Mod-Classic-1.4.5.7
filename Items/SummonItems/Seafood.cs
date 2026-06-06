using CalRD.CalPlayer;
using CalRD.Items.Placeables;
using CalRD.NPCs.AquaticScourge;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.SummonItems
{
    public class Seafood : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Seafood");
/*
            Tooltip.SetDefault("The sulphuric sand stirs...\n" +
                "Summons the Aquatic Scourge");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = 20;
            Item.rare = 5;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            return modPlayer.ZoneSulphur && !NPC.AnyNPCs(ModContent.NPCType<AquaticScourgeHead>());
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            SoundEngine.PlaySound(SoundID.Roar, player.position);
			if (Main.netMode != NetmodeID.MultiplayerClient)
				NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<AquaticScourgeHead>());
			else
				NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<AquaticScourgeHead>());

			return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SulphurousSand>(), 10);
            recipe.AddIngredient(ItemID.Starfish, 5);
            recipe.AddIngredient(ItemID.SharkFin);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
