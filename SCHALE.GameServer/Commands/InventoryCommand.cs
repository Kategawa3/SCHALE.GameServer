﻿using SCHALE.Common.Database;
using SCHALE.Common.FlatData;
using SCHALE.Common.Utils;
using SCHALE.GameServer.Services;
using SCHALE.GameServer.Services.Irc;

namespace SCHALE.GameServer.Commands
{
    [CommandHandler("inventory", "Command to manage inventory (chars, weapons, equipment, items)", "/inventory <addall|clearall>")]
    internal class InventoryCommand : Command
    {
        public InventoryCommand(IrcConnection connection, string[] args, bool validate = true) : base(connection, args, validate) { }

        [Argument(0, @"^addall$|^clearall$", "The operation selected (addall, clearall)", ArgumentFlags.IgnoreCase)]
        public string Op { get; set; } = string.Empty;

        public override void Execute()
        {
            var context = connection.Context;

            switch (Op.ToLower())
            {
                case "addall":
                    InventoryUtils.AddAllCharacters(connection);
                    InventoryUtils.AddAllWeapons(connection);
                    InventoryUtils.AddAllEquipment(connection);
                    InventoryUtils.AddAllItems(connection);
                    break;

                case "clearall":
                    InventoryUtils.RemoveAllCharacters(connection);
                    context.Weapons.RemoveRange(context.Weapons.Where(x => x.AccountServerId == connection.AccountServerId));
                    context.Equipment.RemoveRange(context.Equipment.Where(x => x.AccountServerId == connection.AccountServerId));
                    context.Items.RemoveRange(context.Items.Where(x => x.AccountServerId == connection.AccountServerId));
                    break;
            }

            context.SaveChanges();
        }
    }
}
