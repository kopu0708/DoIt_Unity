using UnityEngine;

public class NotateNumber : MonoBehaviour
{
    public static string Transform (long originNumber)
    {
        // 1,000 = 1k 1,000,000 = 1M
        string[] symbol = new string[7] { "K", "M" ,"G", "T", "E", "P", "Z" };

        // 매개변수로 받은 숫자를 문자열로 변환
        string result = originNumber.ToString();

        // 숫자 + 심벌은 최대 4자리까지 표기, 4자리 이하면 심벌 없이 출력
        if(result.Length < 4)
        {
            return result;
        }
        for(int i = 0; i<symbol.Length; ++i)
        {
            if(4 + 3 * i <= result.Length && result.Length < 4 + 3*(i + 1))
            {
                // 3으로 나누므로 나머지(n)는 0,1,2
                int n = result.Length % 3;
                // 나머지(n)가 0이면 3으로 설정
                n = n == 0 ? 3 : n;

                // 나머지 개수(n) = 옆자리 개수
                // 앞자리 표현에 사용한 값 바로 뒤의 값을 소수점 이하 숫자로 표현
                result = $"{result.Substring(0, n)}.{result.Substring(n, 1)}";
                // 축약 기호 추가
                result += symbol[i];
                break;
            }
        }
        return result;
    }
}
