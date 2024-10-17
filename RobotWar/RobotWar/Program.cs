namespace SambungRobot;

public class Program
{
    public static int turn = 0;
    public const int MaxEnergi = 100;
    public const int MaxArmor = 100;

    static void Main(string[] args)
    {
        RobotTempur RobotRichie = new RobotTempur("RobotRichie", 100, 50, 25);
        RobotTempur RobotTiyo = new RobotTempur("RobotTiyo", 100, 50, 25);
        RobotTempur RobotTsaqif = new RobotTempur("RobotTsaqif", 100, 50, 30);

        BosRobot TeslaBot = new BosRobot("TeslaBot", 300, 60, 40);
        BosRobot HyundaiBot = new BosRobot("HyundaiBot", 200, 60, 35);

        List<Robot> robotTempur = new List<Robot> { RobotRichie, RobotTiyo, RobotTsaqif };
        List<Robot> bosRobot = new List<Robot> { TeslaBot, HyundaiBot };

        Kemampuan electricShock = new SeranganListrik();
        Kemampuan plasmaCannon = new SeranganPlasma();
        Kemampuan ultimateAttack = new SeranganUltimate();
        Kemampuan repair = new Perbaikan();
        Kemampuan superShield = new PertahananSuper();

        while (robotTempur.Exists(r => r.Energi > 0) && bosRobot.Exists(b => b.Energi > 0))
        {
            turn++;

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"\n===== SAMBUNG ROBOT GOAT 2024 =====");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n------------ TURN {turn} ---------------");
            Console.WriteLine("--------- TIM ROBOT KAMU ----------");
            robotTempur.ForEach(r => r.showInfo());

            Console.WriteLine("\n---------- TIM BOS ROBOT ----------");
            bosRobot.ForEach(b => b.showInfo());

            foreach (Robot tempur in robotTempur)
            {
                if (tempur.Energi > 0)
                {
                    bool validAction = false;

                    while (!validAction)
                    {
                        Console.WriteLine($"\nTURN : {tempur.Nama}");
                        Console.WriteLine("1. Serangan Normal");
                        Console.WriteLine("2. Gunakan Kemampuan");

                        int pilihan;
                        while (!int.TryParse(Console.ReadLine(), out pilihan) || pilihan < 1 || pilihan > 2)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Pilihan tidak tersedia");
                            Console.ForegroundColor = ConsoleColor.White;
                        }

                        if (pilihan == 1)
                        {
                            Console.WriteLine("Pilih Bos yang akan diserang: 1. TeslaBot, 2. HyundaiBot");
                            int target;
                            while (!int.TryParse(Console.ReadLine(), out target) || target < 1 || target > 2)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Pilihan tidak tersedia");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            tempur.Serang(target == 1 ? TeslaBot : HyundaiBot);
                            validAction = true;
                        }
                        else if (pilihan == 2)
                        {
                            Console.WriteLine("Pilih Ability: 1. Electric Shock, 2. Plasma Cannon, 3. Ultimate Attack, 4. Repair, 5. Super Shield");
                            int ability;
                            while (!int.TryParse(Console.ReadLine(), out ability) || ability < 1 || ability > 5)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Pilihan tidak tersedia");
                                Console.ForegroundColor = ConsoleColor.White;
                            }

                            if (ability == 1 || ability == 2 || ability == 3)
                            {
                                Console.WriteLine("Pilih Bos yang akan diserang: 1. TeslaBot, 2. HyundaiBot");
                                int target;
                                while (!int.TryParse(Console.ReadLine(), out target) || target < 1 || target > 2)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Pilihan tidak tersedia");
                                    Console.ForegroundColor = ConsoleColor.White;
                                }

                                Robot bosTarget = target == 1 ? TeslaBot : HyundaiBot;
                                Kemampuan selectedAbility = ability == 1 ? electricShock : ability == 2 ? plasmaCannon : ultimateAttack;

                                if (selectedAbility.Cooldown + selectedAbility.LastUse() <= Program.turn)
                                {
                                    tempur.GunakanKemampuan(selectedAbility, bosTarget);
                                    validAction = true;
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Kemampuan sedang dalam cooldown, silakan pilih aksi lain.");
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                            }
                            else if (ability == 4)
                            {
                                if (repair.Cooldown + repair.LastUse() <= Program.turn)
                                {
                                    tempur.GunakanKemampuan(repair, null);
                                    validAction = true;
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Repair sedang dalam cooldown, silakan pilih aksi lain.");
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                            }
                            else if (ability == 5)
                            {
                                if (superShield.Cooldown + superShield.LastUse() <= Program.turn)
                                {
                                    tempur.GunakanKemampuan(superShield, null);
                                    validAction = true;
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Super Shield sedang dalam cooldown, silakan pilih aksi lain.");
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine();

            Random rnd = new Random();
            foreach (Robot bos in bosRobot)
            {
                if (bos.Energi > 0)
                {
                    Robot targetTempur = robotTempur[rnd.Next(robotTempur.Count)];

                    if (rnd.Next(2) == 0)
                    {
                        bos.Serang(targetTempur);
                    }
                    else
                    {
                        int abilityChoice = rnd.Next(3);

                        switch (abilityChoice)
                        {
                            case 0:
                                bos.GunakanKemampuan(electricShock, targetTempur);
                                break;
                            case 1:
                                bos.GunakanKemampuan(ultimateAttack, targetTempur);
                                break;
                            case 2:
                                bos.GunakanKemampuan(plasmaCannon, targetTempur);
                                break;
                        }
                    }
                }
            }

            if (!robotTempur.Exists(r => r.Energi > 0))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Bos Robot Menang, Silahkan coba lagi!");
                Console.ForegroundColor = ConsoleColor.White;
                break;
            }
            if (!bosRobot.Exists(b => b.Energi > 0))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Robot Tempur Kamu Menang Sambung Goat 2024!");
                Console.ForegroundColor = ConsoleColor.White;
                break;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nTurn Selanjutnya! Enter untuk lanjutkan");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();
            Console.Clear();
        }
    }
}