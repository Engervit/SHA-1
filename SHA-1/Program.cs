namespace MySHA
{
    internal class Program
    {
        internal static void Main()
        {
            uint[] K = { 0x5a827999, 0x6ed9eba1, 0x8f1bbcdc, 0xca62c1d6 };
            uint[] H = { 0x67452301, 0xefcdab89, 0x98badcfe, 0x10325476, 0xc3d2e1f0 };
            uint a;
            uint b;
            uint c;
            uint d;
            uint e;

            int count;
            string hash ="";
            uint[] message = new uint[16];
            uint[] messageSchedule = new uint[80];
            Console.Write(" Print the message: ");
            string input = Console.ReadLine();
            if (input.Length % 55 == 0) { count = input.Length / 56; }
            else { count = input.Length / 56 + 1; }
            byte[,] inputParsed = new byte[(count), 64];

            for (int i = 0; i < inputParsed.Length / 64; i++)
            {
                int lenght = 8 * input.Length;

                for (int j = 0; j < 56; j++)
                {
                    if ((57 * i + j) == (input.Length))
                    {
                        inputParsed[i, j] = 0b10000000;
                    }
                    else if ((57 * i + j) > (input.Length - 1))
                    {
                        inputParsed[i, j] = 0;
                    }
                    else
                    {
                        inputParsed[i, j] = (byte)input[56 * i + j];
                    }
                }

                if (i == inputParsed.Length / 64 - 1)
                {
                    for (int j = 63; j > 56; j--) // Problem
                    {
                        int q = 1;
                        if (lenght > 255)
                        {
                            inputParsed[i, j] = (byte)(lenght % (Math.Pow(2, 8) /*+ 1*/));
                        }
                        else
                        {
                            inputParsed[i, j] = (byte)lenght;
                        }

                        lenght = (lenght - inputParsed[i, j]) / 256;
                        q++;
                    }
                }
                else
                {
                    for (int j = 56; j < 64; j++)
                    {
                        if ((57 * i + j) == (input.Length))
                        {
                            inputParsed[i, j] = 0b10000000;
                        }
                        else if ((57 * i + j) > (input.Length - 1))
                        {
                            inputParsed[i, j] = 0;
                        }
                        else
                        {
                            inputParsed[i, j] = (byte)input[56 * i + j];
                        }
                    }
                }
            }

            for (int i = 0; i < inputParsed.Length / 64; i++)
            {
                a = H[0];
                b = H[1];
                c = H[2];
                d = H[3];
                e = H[4];

                for (int t = 0; t < 16; t++)
                {
                    string temp = "";

                    for (int j = 0; j < 4; j++)
                    {
                        string tmp = Convert.ToString(inputParsed[i, 4 * t + j], 2);

                        if (tmp.Length < 8)
                        {
                            for (int r = tmp.Length; r < 8; r++)
                            {
                                tmp = String.Concat(0, tmp);
                            }
                        }
                        temp = String.Concat(temp, tmp); 
                    }

                    message[t] = Convert.ToUInt32(temp, 2);
                }
                //
                for (int t = 0; t < 80; t++)
                {
                    if (t < 16)
                    {
                        messageSchedule[t] = message[t];
                    }
                    else
                    {
                        uint x = messageSchedule[t - 3] ^ messageSchedule[t - 8] ^ messageSchedule[t - 14] ^ messageSchedule[t-16];
                        messageSchedule[t] = (x << 1) | (x >> 31);
                    }
                }

                for (int t = 0; t < 80; t++)
                {
                    uint temp;

                    if (t < 20) 
                    {
                        temp = (uint)( ( ( ( ( ( (ulong)((a << 5) | (a >> 27)) + (ulong)((b & c) ^ (~b & d)) ) % Math.Pow(2, 32) ) + (ulong)e ) % Math.Pow(2, 32)  + (ulong)K[0] ) % Math.Pow(2, 32) + (ulong)messageSchedule[t]) % Math.Pow(2, 32));
                    }
                    else if (t < 40) 
                    {
                        temp = temp = (uint)(((ulong)((a << 5) | (a >> 27)) + (ulong)(b ^ c ^ d) + (ulong)e + (ulong)K[1] + (ulong)messageSchedule[t]) % Math.Pow(2, 32));
                    }
                    else if (t < 60)
                    {
                        temp = temp = (uint)(((ulong)((a << 5) | (a >> 27)) + (ulong)((b & c) ^ (b & d) ^ (c & d)) + (ulong)e + (ulong)K[2] + (ulong)messageSchedule[t]) % Math.Pow(2, 32));
                    }
                    else
                    {
                        temp = temp = (uint)(((ulong)((a << 5) | (a >> 27)) + (ulong)(b ^ c ^ d) + (ulong)e + (ulong)K[3] + (ulong)messageSchedule[t]) % Math.Pow(2, 32));
                    }

                    e = d;
                    d = c;
                    c = ((b << 30) | (b >> 2));
                    b = a;
                    a = temp;
                }

                H[0] = (uint)(((ulong)H[0] + (ulong)a) % Math.Pow(2, 32));
                H[1] = (uint)(((ulong)H[1] + (ulong)b) % Math.Pow(2, 32));
                H[2] = (uint)(((ulong)H[2] + (ulong)c) % Math.Pow(2, 32));
                H[3] = (uint)(((ulong)H[3] + (ulong)d) % Math.Pow(2, 32));
                H[4] = (uint)(((ulong)H[4] + (ulong)e) % Math.Pow(2, 32));
            }

            

            foreach (uint value in H)
            {
                hash = String.Concat(hash, Convert.ToString(value, 16));
            }

            Console.WriteLine(" " + hash);
        }
    }
}