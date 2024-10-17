namespace SambungRobot;

    public abstract class Robot
{
    public string Nama { get; set; }
    public int Energi { get; set; }
    public int Armor { get; set; }
    public int Serangan { get; set; }
    public bool Stunned { get; set; }

    public Robot(string nama, int energi, int armor, int serangan)
    {
        Nama = nama;
        Energi = energi;
        Armor = armor;
        Serangan = serangan;
        Stunned = false;
    }

    public abstract void Serang(Robot target);
    public abstract void Diserang(Robot penyerang);
    public abstract void GunakanKemampuan(Kemampuan kemampuan, Robot target);

    public abstract void showInfo();
}

public class BosRobot : Robot
{
    public BosRobot(string nama, int energi, int armor, int serangan) : base(nama, energi, armor, serangan)
    {

    }

    public override void Serang(Robot target)
    {
        if (!Stunned)
        {
            Console.WriteLine($"{Nama} menyerang {target.Nama}!");
            target.Diserang(this);
        }
        else
        {
            Console.WriteLine($"{Nama} tidak bisa menyerang karena terkena efek stunned.");
        }
    }

    public override void Diserang(Robot penyerang)
    {
        int damage = penyerang.Serangan;
        if (Armor > 0)
        {
            if (damage > Armor)
            {
                int sisaDamage = damage - Armor;
                Armor = 0;
                Energi -= sisaDamage;
            }
            else
            {
                Armor -= damage;
            }
        }

        else
        {
            Energi -= damage;
        }

        if (Energi < 0) Energi = 0;

        Console.WriteLine($"{Nama} menerima {damage} damage. Pertahanan tersisa: {Armor}, Energi tersisa: {Energi}");
    }

    public override void GunakanKemampuan(Kemampuan kemampuan, Robot target)
    {
        if (!Stunned)
        {
            kemampuan.Gunakan(this, target);
        }
        else
        {
            Console.WriteLine($"{Nama} terkena stunned.");
        }
    }

    public void Mati()
    {
        Console.WriteLine($"{Nama} telah mati!");
    }

    public override void showInfo()
    {
        {
            Console.WriteLine();
            Console.WriteLine($"Nama       :{Nama}");
            Console.WriteLine($"Energi     :{Energi}");
            Console.WriteLine($"Pertahanan :{Armor}");
            Console.WriteLine($"Serangan   :{Serangan}");
            Console.WriteLine($"Stunned    :{Stunned}");
        }
    }
}

public class RobotTempur : Robot
{
    public RobotTempur(string nama, int energi, int armor, int serangan) : base(nama, energi, armor, serangan)
    {

    }

    public override void Serang(Robot target)
    {
        if (!Stunned)
        {
            Console.WriteLine($"{Nama} menyerang {target.Nama}!");
            target.Diserang(this);
        }
        else
        {
            Console.WriteLine($"{Nama} terkena stunned.");
        }
    }

    public override void Diserang(Robot penyerang)
    {
        int damage = penyerang.Serangan;
        if (Armor > 0)
        {
            if (damage > Armor)
            {
                int sisaDamage = damage - Armor;
                Armor = 0;
                Energi -= sisaDamage;
            }
            else
            {
                Armor -= damage;
            }
        }

        else
        {
            Energi -= damage;
        }

        if (Energi < 0) Energi = 0;

        Console.WriteLine($"{Nama} terkena {damage} damage. Armor tersisa: {Armor}, Energi tersisa: {Energi}");
    }

    public override void GunakanKemampuan(Kemampuan kemampuan, Robot target)
    {
        if (!Stunned)
        {
            kemampuan.Gunakan(this, target);
        }
        else
        {
            Console.WriteLine($"{Nama} tidak bisa menggunakan kemampuan karena terkena efek stunned.");
        }
    }

    public override void showInfo()
    {
        {
            Console.WriteLine();
            Console.WriteLine($"Nama       :{Nama}");
            Console.WriteLine($"Energi     :{Energi}");
            Console.WriteLine($"Armor      :{Armor}");
            Console.WriteLine($"Serangan   :{Serangan}");
            Console.WriteLine($"Stunned    :{Stunned}");
        }
    }
}

public interface Kemampuan
{
    string Nama();
    int Cooldown { get; set; }
    void Gunakan(Robot pengguna, Robot target = null);
    int LastUse();
}

public class Perbaikan : Kemampuan
{
    public string Nama() => "Repair";
    public int Cooldown { get; set; } = 2;
    private int lastUse = -2;

    public void Gunakan(Robot pengguna, Robot target = null)
    {
        if (lastUse + Cooldown <= Program.turn)
        {
            pengguna.Energi += 30;
            if (pengguna.Energi > Program.MaxEnergi)
                pengguna.Energi = Program.MaxEnergi;
            lastUse = Program.turn;
            Console.WriteLine($"{pengguna.Nama} melakukan perbaikan, memulihkan 30 energi.");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Repair {pengguna.Nama} sedang cooldown.");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    public int LastUse()
    {
        return lastUse;
    }
}

public class SeranganListrik : Kemampuan
{
    public string Nama() => "Electric Shock";
    public int Cooldown { get; set; } = 2;
    private int lastUse = -2;

    public void Gunakan(Robot pengguna, Robot target)
    {
        if (lastUse + Cooldown <= Program.turn)
        {
            target.Stunned = true;
            Console.WriteLine($"{pengguna.Nama} menggunakan Electric Shock kepada {target.Nama}! ");
            Console.WriteLine($"{target.Nama} terkena stun!");
            lastUse = Program.turn;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Electric Shock {pengguna.Nama} sedang cooldown.");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    public int LastUse()
    {
        return lastUse;
    }
}

public class SeranganPlasma : Kemampuan
{
    public string Nama() => "Plasma Cannon";
    public int Cooldown { get; set; } = 3;
    private int lastUse = -3;

    public void Gunakan(Robot pengguna, Robot target)
    {
        if (lastUse + Cooldown <= Program.turn)
        {
            target.Armor = 0;
            Console.WriteLine($"{pengguna.Nama} menggunakan Plasma Cannon kepada {target.Nama} ! ");
            Console.WriteLine($"Armor {target.Nama} telah hancur!");
            lastUse = Program.turn;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Plasma Cannon {pengguna.Nama} sedang cooldown.");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    public int LastUse()
    {
        return lastUse;
    }
}

public class PertahananSuper : Kemampuan
{
    public string Nama() => "Super Shield";
    public int Cooldown { get; set; } = 3;
    private int lastUse = -3;

    public void Gunakan(Robot pengguna, Robot target = null)
    {
        if (lastUse + Cooldown <= Program.turn)
        {
            pengguna.Armor = Program.MaxArmor;
            lastUse = Program.turn;
            Console.WriteLine($"{pengguna.Nama} menggunakan Super Shield, meningkatkan armor.");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Super Shield {pengguna.Nama} sedang cooldown.");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    public int LastUse()
    {
        return lastUse;
    }
}

public class SeranganUltimate : Kemampuan
{
    public string Nama() => "Mini Ultimate Attack Sigma";
    public int Cooldown { get; set; } = 2;
    private int lastUse = -3;

    public void Gunakan(Robot pengguna, Robot target = null)
    {
        if (lastUse + Cooldown <= Program.turn)
        {
            target.Energi -= 50;
            if (target.Energi < 0) target.Energi = 0;
            Console.WriteLine($"{pengguna.Nama} menyerang {target.Nama} dengan Mini Ultimate Attack Sigma!");
            Console.WriteLine($"{target.Nama} terkena 50 damage!");
            lastUse = Program.turn;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Skill {pengguna.Nama} ini sedang cooldown.");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    public int LastUse()
    {
        return lastUse;
    }
}
