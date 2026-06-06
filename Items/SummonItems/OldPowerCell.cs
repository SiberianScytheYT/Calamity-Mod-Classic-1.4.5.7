using CalRD.Items.Materials;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.SummonItems
{
    public class OldPowerCell : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Old Power Cell");
/*
            Tooltip.SetDefault("Summons the ancient golem when used in the Temple");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.maxStack = 20;
            Item.rare = 7;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            bool canSummon = false;
            if ((double)player.Center.Y > Main.worldSurface * 16.0)
            {
                int num = (int)player.Center.X / 16;
                int num2 = (int)player.Center.Y / 16;
                Tile tile = Framing.GetTileSafely(num, num2);
                if (tile.WallType == 87)
                {
                    canSummon = true;
                }
            }
            return canSummon && !NPC.AnyNPCs(NPCID.Golem);
        }

        public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
        {
            SoundEngine.PlaySound(SoundID.Roar, player.position);
			if (Main.netMode != NetmodeID.MultiplayerClient)
				NPC.SpawnOnPlayer(player.whoAmI, NPCID.Golem);
			else
				NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, -1, -1, null, player.whoAmI, NPCID.Golem);

			return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LunarTabletFragment, 10);
            recipe.AddIngredient(ModContent.ItemType<EssenceofCinder>(), 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
