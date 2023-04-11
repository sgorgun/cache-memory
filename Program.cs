namespace CacheMemory;

internal class Cash
{
    public long SizeOfRam { get; set; }
    public long SizeOfBus { get; set; }
    public long WordsInCashLine { get; set; }
    public long CashDivider { get; set; }
    public long K { get; set; }
    public long CashSize { get; set; }
    public long BusOfAddress { get; set; }
    public long Offset { get; set; }
    public long RowSizeInBytes { get; set; }
    public long SetK1 { get; set; }
    public long SetKx { get; set; }
    public long SetFullAssociative { get; set; }
    public long TagK1 { get; set; }
    public long TagKx { get; set; }
    public long TagFullAssociative { get; set; }
    public Cash(long sizeOfRam, long sizeOfBus, long wordsInCashLine, long cashDivider, long k)
    {
        SizeOfRam = sizeOfRam;
        SizeOfBus = sizeOfBus;
        WordsInCashLine = wordsInCashLine;
        CashDivider = cashDivider;
        K = k;
    }
    public void Calculate()
    {
        CashSize = ((SizeOfRam * (long)Math.Pow(2, 30)) / CashDivider) / (long)Math.Pow(2, 20);
        BusOfAddress = (long)Math.Log2(SizeOfRam * (long)Math.Pow(2, 30));
        Offset = (long)Math.Log2(WordsInCashLine);
        RowSizeInBytes = SizeOfBus / 8 * WordsInCashLine;
        SetK1 = (long)Math.Log2(CashSize * (long)Math.Pow(2, 20) / (double)RowSizeInBytes);
        SetKx = SetK1 - (long)Math.Log2(K);
        SetFullAssociative = 0;
        TagK1 = BusOfAddress - SetK1 - Offset;
        TagKx = BusOfAddress - SetKx - Offset;
        TagFullAssociative = BusOfAddress - Offset;
    }
    public void PrintResult()
    {
        Console.WriteLine("\nРезультаты расчета:");
        Console.WriteLine($"1. Размер кэш-памяти составляет: {CashSize}Mb");
        Console.WriteLine($"2. Размер шины адреса составляет: {BusOfAddress}bit");
        Console.WriteLine($"3. Cмещение в строке кэш (OFFSET) составляет: {Offset}bit");
        Console.WriteLine($"4. Размер строки в байтах составляет: {RowSizeInBytes}byte");
        Console.WriteLine($"4.1. SET(k=1): {SetK1}");
        Console.WriteLine($"4.2. SET(k={K}): {SetKx}");
        Console.WriteLine($"4.3. SET(полностью ассоциативный): {SetFullAssociative}");
        Console.WriteLine($"5.1. TAG(k=1): {TagK1}");
        Console.WriteLine($"5.2. TAG(k={K}): {TagKx}");
        Console.WriteLine($"5.3. TAG(полностью ассоциативный): {TagFullAssociative}");
        Console.WriteLine("\nИтоговый результат:");
        Console.WriteLine("\nk=1 (прямой доступ):");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"TAG(k=1): {TagK1}\tSET(k=1): {SetK1}\tOFFSET: {Offset}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"\nk={K} (множ.-ассоц.):");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"TAG(k={K}): {TagKx}\tSET(k={K}): {SetKx}\tOFFSET: {Offset}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\nk = строк в кэше (полностью ассоциативный):");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"TAG(kпа): {TagFullAssociative}\tSET(kпа): {SetFullAssociative}\tOFFSET: {Offset}");
    }
}

internal static class Program
{
    private static void Main()
    {
        while (true)
        {
            var cash = ConditionsCash();
            cash.Calculate();
            cash.PrintResult();
            if (ContinueOrQuit()) break;
        }
    }

    private static Cash ConditionsCash()
    {
        Console.WriteLine("Введите размер ОЗУ в Гб:");
        long sizeOfRam;
        while (!long.TryParse(Console.ReadLine(), out sizeOfRam) && sizeOfRam <= 0)
        {
            Console.WriteLine("Неправильный формат. Введите размер ОЗУ в Гб:");
        }

        Console.WriteLine("Введите размер шины (ШД) в битах:");
        long sizeOfBus;
        while (!long.TryParse(Console.ReadLine(), out sizeOfBus) && sizeOfBus <= 0)
        {
            Console.WriteLine("Неправильный формат. Введите размер шины (ШД) в битах:");
        }

        Console.WriteLine("Введите количество слов в строке:");
        long wordsInCashLine;
        while (!long.TryParse(Console.ReadLine(), out wordsInCashLine) && wordsInCashLine <= 0)
        {
            Console.WriteLine("Неправильный формат. Введите количество слов в строке:");
        }

        Console.WriteLine("Введите делитель КЭШ:");
        long cashDivider;
        while (!long.TryParse(Console.ReadLine(), out cashDivider) && cashDivider <= 0)
        {
            Console.WriteLine("Неправильный формат. Введите делитель КЭШ:");
        }

        Console.WriteLine("Введите k:");
        long k;
        while (!long.TryParse(Console.ReadLine(), out k) && k <= 0)
        {
            Console.WriteLine("Неправильный формат. Введите k:");
        }

        var cash = new Cash(sizeOfRam, sizeOfBus, wordsInCashLine, cashDivider, k);
        return cash;
    }

    private static bool ContinueOrQuit()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\n\nДля выхода из программы нажмите 'q', для того, начать сначала нажмите любую другую клавишу...");
        Console.ForegroundColor = ConsoleColor.White;

        var keyInfo = Console.ReadKey();

        if (keyInfo.KeyChar == 'q')
        {
            return true;
        }

        Console.Clear();
        return false;
    }
}
