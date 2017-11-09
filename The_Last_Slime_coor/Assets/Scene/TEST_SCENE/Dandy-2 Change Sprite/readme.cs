/*
How to use?
1. เอา Game Object Wall กับ Ground ใน Scene Test_Ball ออก 
ยกเว้น Ground Block ที่ใช้เป็น path ของ Guard กับ Wizard
2. เอา Game Object Terrain ใน Scene Dandy-2 ไปใส่แทน

What change?
1. sprite กำแพง เป็นแบบที่ Focus วาด อันล่าสุด
2. เปลี่ยน object กำแพง ให้เป็น prefabs จะได้เปลี่ยน sprite พร้อมกันได้
3. Script WardScript.cs ลบบรรทัด 23 int i; ไม่ได้ใช้ทำอะไร

4. sprite พื้น เป็นแบบที่ Focus วาด อันล่าสุด
5. sprite Wizard, Guard, Slime เป็นแบบที่ Focus วาด อันล่าสุด
6. ใช้ Movewithmouse2.cs ปรับจาก Movewithmouse.cs
gameObject.transform.GetChild(0).gameObject.SetActive
แทนที่ gameObject.GetComponent<SpriteRenderer>().sortingOrder
เพราะใส่ sprite Slime เป็น Child Object จะได้ปรับขนาด sprite ได้

8. sprite Key, Lever, Fire blower, Door, Iron door เป็นแบบที่ Focus วาด อันล่าสุด
9. ใช้ DoorScript2.cs ปรับจาก DoorScript.cs
เปลี่ยน gameObject.GetComponent<SpriteRenderer>().sprite = openSprite;
เป็น
gameObject.transform.GetChild(0).gameObject.SetActive(false);
gameObject.transform.GetChild(1).gameObject.SetActive(true);
+Movewithmouse2.cs บรรทัด 59 ใช้ DoorScript2

-ติด Bug Slime ไม่ยอมตาย ตายเพราะไฟได้อย่างเดียว
-หลบใต้น้ำแล้วแต่ guard ยังวิ่งตาม
*/