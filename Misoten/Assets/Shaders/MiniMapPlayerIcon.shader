Shader "Custom/MiniMapPlayerIcon"
{
	Properties
	{
		// テクスチャを入力したい時の宣言
		// "white"はデフォルトのテクスチャで、それで初期化を行っています
		// 何も書かずに""{}とすることもできます
		_MainTex("Texture", 2D) = ""{}

		// 色を設定するアトリビュートを作る
		_myColor("myColor", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	SubShader
	{
		// カリングと、デプス描画についての設定
		Cull Back ZWrite On ZTest LEqual
		// No culling or depth
		//Cull Off ZWrite Off ZTest Always


		Pass
		{
			CGPROGRAM // 個々からCgでプログラムを記述するという宣言をする
			#pragma vertex vert
			#pragma fragment frag
			
			// 便利な構造体や関数が定義されているinclude
			// 今回は使われてないが、とりあえず書いておくのでも問題ないはず
			#include "UnityCG.cginc"

			// 入力に使う構造体です
			// 頂点入力のPOSITIONに加えて、オブジェクトのuv座標である
			// TEXCOORD0が増えています。
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
		
			};

			// 出力に使う構造体です
			// 入力と見比べると、POSITIONがSV_POSITIONに変わっていますね
			// uvの座標をフラグメントシェーダーで受け取りたいので、TEXCOORD0のセマンティクスが宣言されています
			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
		
			};

			// 入力がappdataになっています
			// また、戻り値の型がfloat4ではなく上で宣言したv2fになっています
			// v2fの中にあるvertexがfloat4でSV_POSITIONなので問題ないわけですね
			v2f vert (appdata v)
			{
				// 構造体を生成
				v2f o;

				// 頂点情報には先ほどと同じく変換した情報を入れます
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

				// uv座標を引き渡しています
				o.uv = v.uv;

				return o;
		
			}

			// 上で宣言したアトリビュート達。書き換えて見たい場合に使ってみてください
			// float4の個別のアトリビュートにアクセスしたい時は
			//_Vector.xのように後ろにつける事で、その要素を扱うことができます
			float4 _myColor;

			// Propertiesで宣言したテクスチャを使えるように宣言しています
			// 2Dテクスチャはsampler2Dで定義します
			sampler2D _MainTex;

			// 戻り値がfixed4になっています。これはfloatより精度の低い小数の型です
			// 頂点シェーダーの出力はフラグメントシェーダーで受け取ることができます
			// これは先ほどのシェーダーには無かった部分ですね
			// (v2f i)の部分で、頂点シェーダーの戻り値と同じ型を宣言しているのがわかります
			// : SV_Targetは新しい表記で、COLORと同じ意味のセマンティクスみたいです（断言できない）
			fixed4 frag (v2f i) : SV_Target
			{
				// 出力するカラーを作っています
				// tex2Dという関数は、Sampler2Dとuv座標を引数にとって、
				// 今描画しようとしているピクセルは、テクスチャのどこのピクセルなのかを計算しています
				fixed4 col = tex2D(_MainTex, i.uv);

				// カラーはRGBAそれぞれ0.0～1.0の範囲になるので1からcolを引くことによって、色が反転します
				//col = 1 - col;

				col.r = col.r * _myColor.r;
				col.g = col.g * _myColor.g;
				col.b = col.b * _myColor.b;
				col.a = col.a * _myColor.a;

				return col;

			}

			ENDCG	// CGPROGRAMの終わりがここという宣言をしている
		}
	}
}
