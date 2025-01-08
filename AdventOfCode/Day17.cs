using FileParser;

namespace AdventOfCode;

public class Day17 : BaseDay
{
    private readonly IParsedFile _input;
    private readonly int[] _program;
    private readonly int _registerA;
    private readonly int _registerB;
    private readonly int _registerC;


    public Day17()
    {
        _input = new ParsedFile(InputFilePath);
        var registerAStr = _input.NextLine();
        var registerBStr = _input.NextLine();
        var registerCStr = _input.NextLine();
        var programStr = _input.NextLine();

        _registerA = registerAStr.ElementAt<int>(2);
        _registerB = registerBStr.ElementAt<int>(2);
        _registerC = registerCStr.ElementAt<int>(2);

        var programStr2 = programStr.ElementAt<string>(1);
        _program = programStr2.Split(',').Select(i => int.Parse(i)).ToArray();


    }

    public override ValueTask<string> Solve_1() => new($"{Solve_1_v1()}");

    private string Solve_1_v1()
    {
        var computer = new Computer();
        computer.Initialize(_registerA, _registerB, _registerC, _program);
        //computer.Initialize(0, 2024, 43690, [4, 0]);
        computer.Start();
        return string.Join(',', computer.Outputs);
    }

    public override ValueTask<string> Solve_2() => throw new NotImplementedException();

    class Computer
    {
        public int RegisterA { get; set; }
        public int RegisterB { get; set; }
        public int RegisterC { get; set; }

        public List<int> Outputs { get; set; }

        public int InstructionPointer { get; set; }

        public int[] Program { get; set; }

        public void Initialize(int registerA, int registerB, int registerC, int[] program)
        {
            RegisterA = registerA;
            RegisterB = registerB;
            RegisterC = registerC;
            Program = program;
            InstructionPointer = 0;
            Outputs = [];
        }

        public void Start()
        {
            while (InstructionPointer < Program.Length)
            {
                var opCode = Program[InstructionPointer];
                var comboOperand = Program[InstructionPointer + 1];
                Process(opCode, comboOperand);
            }
        }

        private void Process(int opCode, int comboOperand)
        {
            var literalValue = comboOperand;
            var comboValue = comboOperand switch
            {
                0 => 0,
                1 => 1,
                2 => 2,
                3 => 3,
                4 => RegisterA,
                5 => RegisterB,
                6 => RegisterC,
                7 => -1,
                _ => throw new InvalidOperationException("Invalid comboOperant")
            };

            switch (opCode)
            {
                case 0:
                    //adv (division)
                    var advResult = (int)(RegisterA / Math.Pow(2, comboValue));
                    RegisterA = advResult;
                    break;
                case 1:
                    //bxl bitwise XOR
                    int bxlResult = RegisterB ^ literalValue;
                    RegisterB = bxlResult;
                    break;
                case 2:
                    //bst modulo 8
                    int bstResult = comboValue % 8;
                    RegisterB = bstResult;
                    break;
                case 3:
                    //jnz 
                    if (RegisterA == 0)
                    {
                        InstructionPointer = int.MaxValue;
                    }
                    else
                    {
                        InstructionPointer = literalValue;
                    }
                    break;
                case 4:
                    //bxc bitwise XOR
                    int bxcResult = RegisterB ^ RegisterC;
                    RegisterB = bxcResult;
                    break;
                case 5:
                    //out 
                    int outResult = comboValue % 8;
                    Outputs.Add(outResult);
                    break;
                case 6:
                    //bdv 
                    var bdvResult = (int)(RegisterA / Math.Pow(2, comboValue));
                    RegisterB = bdvResult;
                    break;
                case 7:
                    //cdv 
                    var cdvResult = (int)(RegisterA / Math.Pow(2, comboValue));
                    RegisterC = cdvResult;
                    break;
            }

            if (opCode != 3)
            {
                InstructionPointer += 2;
            }
        }
    }
}