using CalRD.Items.Materials;
using CalRD.NPCs.Perforator;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.SummonItems
{
    public class BloodyWormFood : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Bloody Worm Food");
/*
            Tooltip.SetDefault("Summons the Perforator Hive");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.maxStack = 20;
            Item.rare = 3;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.ZoneCrimson && !NPC.AnyNPCs(ModContent.NPCType<PerforatorHive>());
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            SoundEngine.PlaySound(SoundID.Roar, player.position);
			if (Main.netMode != NetmodeID.MultiplayerClient)
				NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<PerforatorHive>());
			else
				NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, ModContent.NPCType<PerforatorHive>());

			return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Vertebrae, 9);
            recipe.AddIngredient(ModContent.ItemType<BloodSample>(), 5);
            recipe.AddIngredient(ItemID.CrimtaneBar, 2);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
