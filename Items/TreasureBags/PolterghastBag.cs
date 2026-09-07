using CalRD.Items.Accessories;
using CalRD.Items.Armor.Vanity;
using CalRD.Items.Materials;
using CalRD.Items.PermanentBoosters;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Items.Weapons.Rogue;
using CalRD.Items.Weapons.Summon;
using CalRD.NPCs.Polterghast;
using CalRD.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace CalRD.Items.TreasureBags
{
    public class PolterghastBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Treasure Bag");
/*
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
*/
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 24;
            Item.height = 24;
            Item.rare = 9;
            Item.expert = true;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
			Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalRD/Items/TreasureBags/PolterghastBagGlow").Value);
        }

        public override bool CanRightClick() => true;

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            // Materials
            itemLoot.Add(ModContent.ItemType<RuinousSoul>(), 1, 10, 20);
            itemLoot.Add(ModContent.ItemType<Phantoplasm>(), 1, 20, 30);

            // Weapons
           itemLoot.Add(DropHelper.CalamityStyle(DropHelper.BagWeaponDropRateFraction, new int[]
            {
                ModContent.ItemType<TerrorBlade>(),
                ModContent.ItemType<BansheeHook>(),
                ModContent.ItemType<DaemonsFlame>(),
                ModContent.ItemType<FatesReveal>(),
                ModContent.ItemType<GhastlyVisage>(),
                ModContent.ItemType<EtherealSubjugator>(),
                ModContent.ItemType<GhoulishGouger>()
            }));

            // Equipment
            itemLoot.Add(ModContent.ItemType<Affliction>());
            itemLoot.AddIf(info => CalamityWorld.revenge && !info.player.Calamity().adrenalineBoostThree, ModContent.ItemType<Ectoheart>());

            // Vanity
            itemLoot.Add(ModContent.ItemType<PolterghastMask>(), 7);
        }
    }
}
