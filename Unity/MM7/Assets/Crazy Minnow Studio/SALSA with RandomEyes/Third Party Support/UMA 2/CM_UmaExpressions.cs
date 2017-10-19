using UnityEngine;
using System.Collections;
using UMA;
using UMA.PoseTools;

namespace CrazyMinnow.SALSA.UMA
{
	/// <summary>
	/// RandomEyes3D provides a Custom Shapes system that allows any number of BlendShapes to be linked to the system
	/// and made accessible via the RandomEyes3D API. However, since UMA does not use BlendShapes, this script 
	/// provides key RandomEyes3D Custom Shapes like behavior for the UMAExpressionPlayer facial expressions.
	/// 
	/// Instructions: 
	/// 1. Add this script alongside the CM_UmaSync.cs script.
	/// 
	/// 2. From your custom script, get reference to this script using standard Unity methods.
	///		using CrazyMinnow.SALSA.UMA;
	/// 
	///		public CM_UmaExpressions umaExpressionHelper;
	///		
	///		void Start()
	///		{
	///			if (!umaExpressionHelper) 
	///			{
	///				umaExpressionHelper = GetComponent<CM_UmaExpressions>();
	///			}
	///		}
	/// 
	/// 3. Call one of the two available public SetExpression methods.
	///		To activate an expression for a specified duration:
	/// 
	///		umaExpressionHelper.SetExpression(CM_UmaExpressions.Expression.leftMouthSmile_Frown, 3.5f, 1f, 2f);
	///	
	/// 		Expression to activate: umaExpressionHelper.Expression.leftMouthSmile_Frown
	///			BlendSpeed: How quickly the expression will reach it's specified amount
	///			Amount: From -1 to 1, with 0 being no expression
	///			Duration: How long to hold the expression once the target amount has been reached
	/// 		
	///		To activate or deactivate an expression without any specified duration:
	///	
	/// 	umaExpressionHelper.SetExpression(CM_UmaExpressions.Expression.leftMouthSmile_Frown, 3.5f, 1f, true);
	/// 
	///			Expression to activate: umaExpressionHelper.Expression.leftMouthSmile_Frown
	///			BlendSpeed: How quickly the expression will reach it's specified amount
	///			Amount: From -1 to 1, with 0 being no expression
	///			IsOn: Does not use a timer, the expression either blends to on or off.
	/// </summary>
	[AddComponentMenu("Crazy Minnow Studio/UMA 2/CM_UmaExpressions")]
	public class CM_UmaExpressions : MonoBehaviour
	{
		public UMAExpressionPlayer umaExpressionPlayer;
		public enum Expression
		{
			jawOpen_Close, jawForward_Back, jawLeft_Right, mouthLeft_Right,
			mouthUp_Down, mouthNarrow_Pucker, tongueOut, tongueUp_Down, tongueLeft_Right, tongueWide_Narrow,
			leftMouthSmile_Frown, rightMouthSmile_Frown, leftLowerLipUp_Down, rightLowerLipUp_Down,
			leftUpperLipUp_Down, rightUpperLipUp_Down, leftCheekPuff_Squint, rightCheekPuff_Squint,
			noseSneer, browsIn, leftBrowUp_Down, rightBrowUp_Down, midBrowUp_Down
		}

		/// <summary>
		/// Gets reference tot he local UMAExpressionPlayer
		/// </summary>
		void Update()
		{
			if (!umaExpressionPlayer)
			{
				umaExpressionPlayer = GetComponent<UMAExpressionPlayer>();
				if (!umaExpressionPlayer)
				{
					umaExpressionPlayer = GetComponentInChildren<UMAExpressionPlayer>();
				}
			}
		}

		/// <summary>
		/// SetExpression is a public method that allows you to smoothly blend any of the 
		/// supported UMA expressions to the specified amount, at the specified blend speed, 
		/// and for the specified duration.
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="blendSpeed"></param>
		/// <param name="amount"></param>
		/// <param name="duration"></param>
		public void SetExpression(Expression expression, float blendSpeed, float rangeOfMotion, float duration)
		{
            StartCoroutine(setExpression(expression, blendSpeed, rangeOfMotion, duration));
		}

		/// <summary>
		/// setExpression is a private IEnumerator method that's called from SetExpression,
		/// and allows you to smoothly blend any of the supported UMA expressions to the 
		/// specified amount, at the specified blend speed, and for the specified duration.
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="blendSpeed"></param>
		/// <param name="amount"></param>
		/// <param name="duration"></param>
		/// <returns></returns>
		IEnumerator setExpression(Expression expression, float blendSpeed, float rangeOfMotion, float duration)
		{
			float startAmt = GetValue(expression);
			float currAmt = startAmt;
			bool done;

			done = false;
			while (!done)
			{
				currAmt = Mathf.Lerp(currAmt, rangeOfMotion, Time.deltaTime * blendSpeed);
				SetValue(expression, currAmt);
				if (currAmt.ToString("F2") == rangeOfMotion.ToString("F2"))
				{
					done = true;
				}
				yield return null;
			}

			yield return new WaitForSeconds(duration);

			done = false;
			while (!done)
			{
				currAmt = Mathf.Lerp(currAmt, startAmt, Time.deltaTime * blendSpeed);
				SetValue(expression, currAmt);
				if (currAmt.ToString("F2") == startAmt.ToString("F2"))
				{
					done = true;
				}
				yield return null;
			}
		}

		/// <summary>
		/// SetExpression is a public method that allows you to smoothly blend any of the 
		/// supported UMA expressions to the specified amount, at the specified blend speed.
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="blendSpeed"></param>
		/// <param name="amount"></param>
		/// <param name="isOn"></param>
		public void SetExpression(Expression expression, float blendSpeed, float rangeOfMotion, bool isOn)
		{
			StartCoroutine(setExpression(expression, blendSpeed, rangeOfMotion, isOn));
		}

		/// <summary>
		/// setExpression is a private IEnumerator method that's called from SetExpression,
		/// and allows you to smoothly blend any of the supported UMA expressions to the 
		/// specified amount, at the specified blend speed.
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="blendSpeed"></param>
		/// <param name="amount"></param>
		/// <param name="isOn"></param>
		/// <returns></returns>
		IEnumerator setExpression(Expression expression, float blendSpeed, float rangeOfMotion, bool isOn)
		{
			float zero = 0f;
			float startAmt = GetValue(expression);
			float currAmt = startAmt;
			bool done;

			if (isOn)
			{
				done = false;
				while (!done)
				{
					currAmt = Mathf.Lerp(currAmt, rangeOfMotion, Time.deltaTime * blendSpeed);
					SetValue(expression, currAmt);
					if (currAmt.ToString("F1") == rangeOfMotion.ToString("F1"))
					{
						done = true;
					}
					yield return null;
				}
			}

			if (!isOn)
			{
				done = false;
				while (!done)
				{
					currAmt = Mathf.Lerp(currAmt, zero, Time.deltaTime * blendSpeed);
					SetValue(expression, currAmt);
					if (currAmt.ToString("F1") == zero.ToString("F1"))
					{
						done = true;
					}
					yield return null;
				}
			}
		}

		/// <summary>
		/// Get an UMAExpressionPlayer expression value
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		private float GetValue(Expression expression)
		{
			float amount = 0f;

			switch (expression)
			{
				case Expression.jawOpen_Close:
					amount = umaExpressionPlayer.jawOpen_Close;
					break;
				case Expression.jawForward_Back:
					amount = umaExpressionPlayer.jawForward_Back;
					break;
				case Expression.jawLeft_Right:
					amount = umaExpressionPlayer.jawLeft_Right;
					break;
				case Expression.mouthLeft_Right:
					amount = umaExpressionPlayer.mouthLeft_Right;
					break;
				case Expression.mouthUp_Down:
					amount = umaExpressionPlayer.mouthUp_Down;
					break;
				case Expression.mouthNarrow_Pucker:
					amount = umaExpressionPlayer.mouthNarrow_Pucker;
					break;
				case Expression.tongueOut:
					amount = umaExpressionPlayer.tongueOut;
					break;
				case Expression.tongueUp_Down:
					amount = umaExpressionPlayer.tongueUp_Down;
					break;
				case Expression.tongueLeft_Right:
					amount = umaExpressionPlayer.tongueLeft_Right;
					break;
				case Expression.tongueWide_Narrow:
					amount = umaExpressionPlayer.tongueWide_Narrow;
					break;
				case Expression.leftMouthSmile_Frown:
					amount = umaExpressionPlayer.leftMouthSmile_Frown;
					break;
				case Expression.rightMouthSmile_Frown:
					amount = umaExpressionPlayer.rightMouthSmile_Frown;
					break;
				case Expression.leftLowerLipUp_Down:
					amount = umaExpressionPlayer.leftLowerLipUp_Down;
					break;
				case Expression.rightLowerLipUp_Down:
					amount = umaExpressionPlayer.rightLowerLipUp_Down;
					break;
				case Expression.leftUpperLipUp_Down:
					amount = umaExpressionPlayer.leftUpperLipUp_Down;
					break;
				case Expression.rightUpperLipUp_Down:
					amount = umaExpressionPlayer.rightUpperLipUp_Down;
					break;
				case Expression.leftCheekPuff_Squint:
					amount = umaExpressionPlayer.leftCheekPuff_Squint;
					break;
				case Expression.rightCheekPuff_Squint:
					amount = umaExpressionPlayer.rightCheekPuff_Squint;
					break;
				case Expression.noseSneer:
					amount = umaExpressionPlayer.noseSneer;
					break;
				case Expression.browsIn:
					amount = umaExpressionPlayer.browsIn;
					break;
				case Expression.leftBrowUp_Down:
					amount = umaExpressionPlayer.leftBrowUp_Down;
					break;
				case Expression.rightBrowUp_Down:
					amount = umaExpressionPlayer.rightBrowUp_Down;
					break;
				case Expression.midBrowUp_Down:
					amount = umaExpressionPlayer.midBrowUp_Down;
					break;
			}

			return amount;
		}

		/// <summary>
		/// Set an UMAExpressionPlayer expression value
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="amount"></param>
		private void SetValue(Expression expression, float amount)
		{
			switch (expression)
			{
				case Expression.jawOpen_Close:
					umaExpressionPlayer.jawOpen_Close = amount;
					break;
				case Expression.jawForward_Back:
					umaExpressionPlayer.jawForward_Back = amount;
					break;
				case Expression.jawLeft_Right:
					umaExpressionPlayer.jawLeft_Right = amount;
					break;
				case Expression.mouthLeft_Right:
					umaExpressionPlayer.mouthLeft_Right = amount;
					break;
				case Expression.mouthUp_Down:
					umaExpressionPlayer.mouthUp_Down = amount;
					break;
				case Expression.mouthNarrow_Pucker:
					umaExpressionPlayer.mouthNarrow_Pucker = amount;
					break;
				case Expression.tongueOut:
					umaExpressionPlayer.tongueOut = amount;
					break;
				case Expression.tongueUp_Down:
					umaExpressionPlayer.tongueUp_Down = amount;
					break;
				case Expression.tongueLeft_Right:
					umaExpressionPlayer.tongueLeft_Right = amount;
					break;
				case Expression.tongueWide_Narrow:
					umaExpressionPlayer.tongueWide_Narrow = amount;
					break;
				case Expression.leftMouthSmile_Frown:
					umaExpressionPlayer.leftMouthSmile_Frown = amount;
					break;
				case Expression.rightMouthSmile_Frown:
					umaExpressionPlayer.rightMouthSmile_Frown = amount;
					break;
				case Expression.leftLowerLipUp_Down:
					umaExpressionPlayer.leftLowerLipUp_Down = amount;
					break;
				case Expression.rightLowerLipUp_Down:
					umaExpressionPlayer.rightLowerLipUp_Down = amount;
					break;
				case Expression.leftUpperLipUp_Down:
					umaExpressionPlayer.leftUpperLipUp_Down = amount;
					break;
				case Expression.rightUpperLipUp_Down:
					umaExpressionPlayer.rightUpperLipUp_Down = amount;
					break;
				case Expression.leftCheekPuff_Squint:
					umaExpressionPlayer.leftCheekPuff_Squint = amount;
					break;
				case Expression.rightCheekPuff_Squint:
					umaExpressionPlayer.rightCheekPuff_Squint = amount;
					break;
				case Expression.noseSneer:
					umaExpressionPlayer.noseSneer = amount;
					break;
				case Expression.browsIn:
					umaExpressionPlayer.browsIn = amount;
					break;
				case Expression.leftBrowUp_Down:
					umaExpressionPlayer.leftBrowUp_Down = amount;
					break;
				case Expression.rightBrowUp_Down:
					umaExpressionPlayer.rightBrowUp_Down = amount;
					break;
				case Expression.midBrowUp_Down:
					umaExpressionPlayer.midBrowUp_Down = amount;
					break;
			}
		}
	}
}