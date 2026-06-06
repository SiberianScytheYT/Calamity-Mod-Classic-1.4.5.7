using CalRD.Items.Materials;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.SummonItems
{
    public class BulbofDoom : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Portabulb");
/*
            Tooltip.SetDefault("Summons Plantera");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.maxStack = 20;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.rare = 7;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ZoneJungle && !NPC.AnyNPCs(NPCID.Plantera);
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            SoundEngine.PlaySound(SoundID.Roar, player.position);
			if (Main.netMode != NetmodeID.MultiplayerClient)
				NPC.SpawnOnPlayer(player.whoAmI, NPCID.Plantera);
			else
				NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, NPCID.Plantera);

			return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.JungleSpores, 15);
            recipe.AddIngredient(ItemID.SoulofNight, 10);
            recipe.AddIngredient(ItemID.SoulofLight, 10);
            recipe.AddIngredient(ModContent.ItemType<MurkyPaste>(), 3);
            recipe.AddIngredient(ItemID.Vine);
            recipe.AddIngredient(ModContent.ItemType<TrapperBulb>());
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
