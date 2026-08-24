using CalRD.CalPlayer;
using CalRD.Projectiles.Typeless;
using CalRD.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    //Dedicated to Dzicozan
    [AutoloadEquip(EquipType.Back)]
    public class TheCamper : ModItem
    {
        int auraCounter = 0;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("The Camper");
/*
            Tooltip.SetDefault("In rest may we find victory.\n" +
				"You deal no damage unless stationary\n" +
                "Standing still grants buff(s) dependent on what weapon you're holding\n" +
                "Standing still provides a damaging aura around you\n" +
                "While moving, you regenerate health as if standing still\n" +
				"Provides a small amount of light in the Abyss");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CalamityGlobalItem.Rarity7BuyPrice; 
            Item.rare = 7;
            Item.accessory = true;
            Item.defense = 10;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
			if (CalamityWorld.death)
			{
				foreach (TooltipLine line2 in list)
				{
					if (line2.Mod == "Terraria" && line2.Name == "Tooltip5")
					{
						line2.Text = "Provides a small amount of light in the Abyss\n" +
						"Provides cold protection in Death Mode";
					}
				}
			}
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.camper = true;
            player.AddBuff(BuffID.HeartLamp, 60, true);
            player.AddBuff(BuffID.Campfire, 60, true);
            player.AddBuff(BuffID.WellFed, 60, true);
            player.lifeRegen += 2;
            Lighting.AddLight(player.Center, 0.825f, 0.66f, 0f);
            if (Main.myPlayer == player.whoAmI)
            {
                if (player.StandingStill())
                {
                    player.GetDamage(DamageClass.Generic) += 0.15f;
                    auraCounter++;
                    float range = 200f;
                    if (auraCounter == 9)
                    {
                        auraCounter = 0;
                        for (int i = 0; i < Main.maxNPCs; i++)
                        {
                            NPC npc = Main.npc[i];
                            if (npc.active && !npc.friendly && npc.damage > -1 && !npc.dontTakeDamage && Vector2.Distance(player.Center, npc.Center) <= range)
                            {
                                Projectile p = Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), npc.Center, Vector2.Zero, ModContent.ProjectileType<DirectStrike>(), (int)(Main.rand.Next(20, 41) * player.AverageDamage()), 0f, player.whoAmI, i);
                                if (!npc.buffImmune[BuffID.OnFire])
                                {
                                    npc.AddBuff(BuffID.OnFire, 120);
                                }
                            }
                        }
                    }
                    if (player.ActiveItem() != null && !player.ActiveItem().IsAir && player.ActiveItem().stack > 0)
                    {
                        bool summon = player.ActiveItem().CountsAsClass(DamageClass.Summon);
                        bool rogue = player.ActiveItem().Calamity().rogue;
                        bool melee = player.ActiveItem().CountsAsClass(DamageClass.Melee);
                        bool ranged = player.ActiveItem().CountsAsClass(DamageClass.Ranged);
                        bool magic = player.ActiveItem().CountsAsClass(DamageClass.Magic);
                        if (summon)
                        {
                            player.GetKnockback(DamageClass.Summon) += 0.10f;
                            player.AddBuff(BuffID.Bewitched, 60, true);
                        }
                        else if (rogue)
                        {
                            modPlayer.throwingVelocity += 0.10f;
                        }
                        else if (melee)
                        {
                            player.GetAttackSpeed(DamageClass.Melee) += 0.10f;
                            player.AddBuff(BuffID.Sharpened, 60, true);
                        }
                        else if (ranged)
                        {
                            player.AddBuff(BuffID.AmmoBox, 60, true);
                        }
                        else if (magic)
                        {
                            player.AddBuff(BuffID.Clairvoyance, 60, true);
                        }
                    }
                }
                else
                {
                    auraCounter = 0;
                }
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Campfire, 10);
            recipe.AddIngredient(ItemID.HeartLantern, 5);
            recipe.AddRecipeGroup("AnyFood", 50);
            recipe.AddIngredient(ItemID.ShinyStone);
            recipe.AddIngredient(ItemID.SharpeningStation);
            recipe.AddIngredient(ItemID.CrystalBall);
            recipe.AddIngredient(ItemID.AmmoBox);
            recipe.AddIngredient(ItemID.BewitchingTable);

            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();

        }
    }
}
