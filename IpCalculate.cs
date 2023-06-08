using System;

namespace NetworkCalculate
{
    public class IpCalculate
    {
        //Адрес сети
        private string _ipAddress;
        //Маска сети формат записи /28
        private int _mask;
        //Маска сети формата 255.255.240.0
        private string _maskFormatStr;
        //--------------------------
        private string network;
        private string wildcard;
        private string broadcast;
        private string minAddress;
        private string maxAddress;

        //Иницилизация данных
        public IpCalculate(string ipAddress, string mask)
        {
            _ipAddress = ipAddress;
            _maskFormatStr = mask;
            RunCalculate();
        }
        //Иницилизация данных
        public IpCalculate(string ipAddress, int mask)
        {
            _ipAddress = ipAddress;
            _mask = mask;
            RunCalculate();
        }
        //Метод запуск рассчета сети
        private void RunCalculate()
        {
            byte[] resultIp = GetIpListBytes();
            byte[] resultMask = GetMaskListBytes();
            //Получим минимальный адрес
            minAddress = GetMinAddress(resultIp, resultMask);
            //Получим максимальный адрес
            maxAddress = GetMaxAddress(resultIp, resultMask);
            //Получим широковещательный адрес
            broadcast = GetBroadcastAddress(resultIp, resultMask);
            //Получим адрес сети
            network = GetNetworkAddress(resultIp, resultMask);
            //Получим обратную маску сети
            wildcard = GetWildcardAddress(resultMask);
        }
        private byte[] GetMaskListBytes()
        {
            byte[] data = new byte[32];
            //Выполним условие если в маске указан формат виде /'28', а не '255.255.240.0'.
            if (_mask != 0)
            {
                //Далее заполняем массив
                for (int i = 0; i < data.Length; i++)
                {
                    if (_mask > i)
                        data[i] = 1;
                    else
                        data[i] = 0;
                }
            }

            //Выполним условие если в маске указан формат виде '255.255.240.0', а не /'28'.
            if (_maskFormatStr != null)
            {
                //Получим список октетов
                string[] listOctet = _maskFormatStr.Split('.');
                string result = "";
                int count = 0;

                for (int i = 0; i < listOctet.Length; i++)
                {
                    //Получим каждый октет в двоичной ввиде
                    result = Convert.ToString(int.Parse(listOctet[i]), 2);
                    //Запишим в результат в массив
                    for (int j = 0; j < result.Length; j++)
                    {
                        if (count < 32)
                        {
                            data[count] = Convert.ToByte(result[j].ToString());
                            count++;
                        }
                    }
                }

            }

            return data;
        }
        private byte[] GetIpListBytes()
        {
            //Массив Содержащий все 4 октета в двоичной системе
            byte[] data = new byte[32];
            //Массив 1 октета
            byte[] octet = new byte[8];
            //Счетчик для заполнения все 4 октетов
            int sw = 0;
            int number = 0;
            string[] ip = _ipAddress.Split('.');

            for (int i = 0; i < ip.Length; i++)
            {
                //Хранит число 1 октета
                number = int.Parse(ip[i]);
                //В цикле из целого десятичного числа октета получим двоичное число
                for (int j = 0; j < 8; j++)
                {
                    octet[j] = Convert.ToByte(number % 2);
                    number /= 2;
                }
                Array.Reverse(octet);
                //Запишим все 4 октета двоичном виде в общий массив
                for (int j = 0; j < 8; j++)
                {
                    data[sw] = octet[j];
                    sw += 1;
                }
            }

            return data;
        }
        //Получим адрес в десятичном виде. Например 192.168.1.2
        private string GetBinToDec(byte[] data)
        {
            int[] numbers = { 128, 64, 32, 16, 8, 4, 2, 1 };
            int[] octets = new int[4];
            int result = 0;
            int n = 0;
            int m = 0;

            for (int i = 0; i < 32; i += 8)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (data[m++] == 1)
                        result += numbers[j];
                }
                octets[n++] = result;
                result = 0;
            }
            return String.Format("{0}.{1}.{2}.{3}", octets[0], octets[1], octets[2], octets[3]);
        }
        //Широковещательный адрес
        private string GetBroadcastAddress(byte[] ip, byte[] mask)
        {
            for (int i = 0; i < 32; i++)
                if (mask[i] == 0)
                    ip[i] = 1;
            return GetBinToDec(ip);
        }
        //Минимальный адрес сети
        private string GetMinAddress(byte[] ip, byte[] mask)
        {
            for (int i = 0; i < 32; i++)
                if (mask[i] == 0)
                    ip[i] = 0;
            ip[31] = 1;
            return GetBinToDec(ip);
        }
        //Максимальный адрес сети
        private string GetMaxAddress(byte[] ip, byte[] mask)
        {
            for (int i = 0; i < 32; i++)
                if (mask[i] == 0)
                    ip[i] = 1;
            ip[31] = 0;
            return GetBinToDec(ip);
        }
        //Адрес сети
        private string GetNetworkAddress(byte[] ip, byte[] mask)
        {
            for (int i = 0; i < 32; i++)
                if (mask[i] == 0)
                    ip[i] = 0;
            return GetBinToDec(ip);
        }
        //Обратная маска сети Wildcard
        private string GetWildcardAddress(byte[] mask)
        {
            for (int i = 0; i < 32; i++)
                if (mask[i] == 0)
                    mask[i] = 1;
                else
                    mask[i] = 0;
            return GetBinToDec(mask);
        }

        public string MinAddress
        {
            get { return minAddress; }
        }

        public string MaxAddress
        {
            get { return maxAddress; }
        }

        public string Broadcast
        {
            get { return broadcast; }
        }

        public string Network
        {
            get { return network; }
        }

        public string Wildcard
        {
            get { return wildcard; }
        }
    }
}