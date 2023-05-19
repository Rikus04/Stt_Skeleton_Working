public class Packer
    {
        static void Main(string[] args)
        {
            string filePath = "C:\\Users\\Rikus\\source\\repos\\ConsoleApp4\\ConsoleApp4\\example_input";
            string packedItems = pack(filePath);
            Console.WriteLine(packedItems);
        }

        static string pack(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            string result = "";

            foreach (string line in lines)
            {
                // parse package and items
                string[] items = line.Substring(line.IndexOf(':') + 2).Replace(", ", ",").Split(' ');
                double[] weights = new double[items.Length];
                double[] costs = new double[items.Length];
                int[] indices = new int[items.Length];

                for (int i = 1; i <= items.Length; i++)
                {
                    string item = items[i - 1];
                    string[] itemParts = item.Split(',');

                    indices[i - 1] = int.Parse(itemParts[0].Replace("(", ""));
                    weights[i - 1] = double.Parse(itemParts[1], CultureInfo.InvariantCulture);
                    costs[i - 1] = double.Parse(itemParts[2].Replace("€", "").Replace(")", ""), CultureInfo.InvariantCulture);
                }

                int n = weights.Length;
                int weightAllowed = (int)(double.Parse(line.Substring(0, line.IndexOf(':'))) * 100); // Multiply by 100 to avoid floating point issues
                int[,] K = new int[n + 1, weightAllowed + 1]; // Multiply by 100 to avoid floating point issues

                for (int i = 0; i <= n; i++)
                {
                    for (int w = 0; w <= weightAllowed; w++)
                    {
                        if (i == 0 || w == 0)
                            K[i, w] = 0;
                        else if ((int)(weights[i - 1] * 100) <= w) // Make sure the weight doesn't exceed `w`
                            K[i, w] = Math.Max((int)(costs[i - 1] * 100) + K[i - 1, w - (int)(weights[i - 1] * 100)], K[i - 1, w]);
                        else
                            K[i, w] = K[i - 1, w];
                    }
                }

                List<int> itemsSelected = new List<int>();
                int res = K[n, weightAllowed];
                int wgt = weightAllowed;

                for (int i = n; i > 0 && res > 0; i--)
                {
                    if (res == K[i - 1, wgt])
                        continue;
                    else
                    {
                        itemsSelected.Add(indices[i - 1]);

                        res = res - (int)(costs[i - 1] * 100);
                        wgt = wgt - (int)(weights[i - 1] * 100);
                    }
                }

                result += string.Join(",", itemsSelected) + "\n";
            }

            return result;
        }
    }