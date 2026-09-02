using UnityEngine;

public static class Utils 
{
    /// <summary> 
    /// 회전값이 Vector3.zero라면 방향을 보는 오브젝트 기준이고
    /// 방향이 다르면 weight에 각도를 더한다.
    /// 반시계 방향 기준 +, 시계 방향 기준 - 
    /// </summary>
    public static Quaternion RotateToTarget(Vector2 owner, Vector2 target, float weight = 0)
    {
        // 원점에서 거리, 수평축에서 각도를 이용해 위치를 구하는 극좌표계 이용
        // 각도 = arctan(y/x)
        // x, y 변윗값 구하기 
        float dx = target.x - owner.x;
        float dy = target.y - owner.y;

        // x, y 변윗값을 바탕으로 각도 구하기 
        // 각도가 raidus 단위이므로 Mathf.Rad2Deg를 곱해 각도 단위를 구함 
        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        return Quaternion.Euler(0, 0,degree - weight);
    }

    public static float GetAngleFromPosition(Vector2 owner, Vector2 target)
    {
        // 원점에서의 거리, 수평축에서의 각도로 위치를 구하는 극좌표게 이용
        // 각도 = arctan(y/x)
        // x, y 변위값 구하기 
        float dx = target.x - owner.x;
        float dy = target.y - owner.y;

        // x, y 변윗값을 바탕으로 각도 구하기 
        // 각도가 radian 단위이므로 Mathf.Rad2Deg를 곱해 각도 단위를 구함
        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        // return Mathf.Atan(Mathf.Abs(owner.y - target.y) / Mathf.Abs(owner.x - target.x));

        return degree;
    }
    /// <summary>
    /// 각도를 기준으로 원의 둘레 위치를 구한다.
    /// </summary>
    /// <param name="radius">원의 반지름 </param>
    /// <param name="angle">각도</param>
    /// <returns>원의 반지름, 각도에 해당하는 둘레 위치</returns>
    public static Vector3 GetPositionFromAngle(float radius,float angle)
    {
        Vector3 position = Vector3.zero;

        angle = DegreeToRadian(angle);

        position.x = Mathf.Cos(angle) * radius;
        position.y = Mathf.Sin(angle) * radius; 

        return position;
    }
    /// <summary>
    /// Degree값을 Radian값으로 변환한다.
    /// 1도는 "PI/180 * angle" radian
    /// </summary>
    public static float DegreeToRadian(float angle)
    {
        return Mathf.PI * angle / 180;
    }

    public static Vector2 GetNewPoint(Vector3 start, float angle, float r)
    {
        // Degree 각도를 Radius으로 변경
        angle = DegreeToRadian(angle);

        // 원점을 기준으로 X, Y좌표를 구하므로 시작 좌표 (start)를 더함 원점 기준이 아니라 임의의 시작점을 기준으로 하기 위해
        Vector2 position = Vector2.zero;
        position.x = Mathf.Cos(angle) * r + start.x;  
        position.y = Mathf.Sin(angle) * r + start.y;

        return position;
    }

    public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
    {
        return a + (b - a) * t;  // 벡터 연산에 비율(t)를 곱해 그 방향으로 t% 만큼 이동한 좌표를 구한다.
    }

    public static Vector2 QuadraticCurve(Vector2 a, Vector2 b, Vector2 c, float t) //  Lerp함수를 중첩 사용해 직선 이동을 여러변 겹쳐 곡선을 만든다.
    {
        Vector2 p1 = Lerp(a, b, t);  
        Vector2 p2 = Lerp(b, c, t);

        return Lerp(p1, p2, t);
    }

    public static Vector2 CubicCurve(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t)
    {
        Vector2 p1 = QuadraticCurve(a, b, c, t);
        Vector2 p2 = QuadraticCurve(b, c, d, t);

        return Lerp(p1, p2, t);
    }
}
