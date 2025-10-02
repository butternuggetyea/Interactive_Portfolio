﻿
// AliyerEdon@gmail.com/
// Lighting Box is an "paid" asset. Don't share it for free

#if UNITY_EDITOR   
using UnityEngine;   
using System.Collections;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;   
using UnityEngine.Rendering.PostProcessing;
using LightingBox.Effects;
using cCharkes;
namespace LightingBox.Effects
{
	[ExecuteInEditMode]
	public class LB_LightingBox : EditorWindow
	{
		#region Variables
		public AutoProfile autoProfile;

		public WindowMode winMode = WindowMode.Part1;
		public LB_LightingBoxHelper helper;
		public Target_Platform targetPlatform = Target_Platform.FullFeatured;

		// Sun Shaft
		public SunShafts.SunShaftsResolution shaftQuality = SunShafts.SunShaftsResolution.High;
		public float shaftDistance = 0.5f;
		public float shaftBlur = 4f;
		public Color shaftColor = new Color32(255, 189, 146, 255);

		// AA
		public AAMode aaMode;

		// AO
		public AOType aoType;
		public float aoRadius;
		public float aoIntensity = 1f;
		public bool ambientOnly = true;
		public Color aoColor = Color.black;
		public AmbientOcclusionQuality aoQuality = AmbientOcclusionQuality.Medium;

		// Bloom
		public float bIntensity = 1f;
		public float bThreshould = 0.5f;
		public Color bColor = Color.white;
		public Texture2D dirtTexture;
		public float dirtIntensity;
		public bool mobileOptimizedBloom;
		public float bRotation;

		public bool visualize;

		// Color settings
		public float exposureIntensity = 1.43f;
		public float contrastValue = 30f;
		public float temp = 0;
		public ColorMode colorMode;
		public float gamma = 0;
		public Color colorGamma = Color.black;
		public Color colorLift = Color.black;
		public float saturation = 0;
		public Texture lut;

		// Auto Exposure
		public LB_ExposureMode exposureMode = LB_ExposureMode.Optimal;
		public float exposureCompensation = 0.17f;
		public float eyeMin = -6f;
		public float eyeMax = -3f;

		// SSR
		public ScreenSpaceReflectionPreset ssrQuality = ScreenSpaceReflectionPreset.Lower;
		public float ssrAtten;
		public float ssrFade;

		// Stochastic SSR    
		public ResolutionMode resolutionMode = ResolutionMode.halfRes;
		public SSRDebugPass debugPass = SSRDebugPass.Combine;
		public int rayDistance = 70;
		public float screenFadeSize = 0;
		public float smoothnessRange = 1f;

		public float vignetteIntensity = 0.1f;
		public float CA_Intensity = 0.1f;
		public bool mobileOptimizedChromattic;

		public Render_Path renderPath;

		// Profiles
		public LB_LightingProfile LB_LightingProfile;
		public PostProcessProfile postProcessingProfile;

		public LightingMode lightingMode;
		public AmbientLight ambientLight;
		public LightSettings lightSettings;
		public LightProbeMode lightprobeMode;

		// Depth of field
		public float focusDistance;
		public float AperTure;
		public float fLength;
		public UnityEngine.Rendering.PostProcessing.KernelSize kernelSize;

		// Sky and Sun
		public Material skyBox;
		public Cubemap skyCube;
		public float skyBoxExposure = 1f;
		public Light sunLight;
		public Flare sunFlare;
		public Color sunColor = Color.white;
		public float sunIntensity = 2.1f;
		public float indirectIntensity = 1.43f;
		public Color ambientColor = new Color32(96, 104, 116, 255);
		public Color skyColor;
		public Color equatorColor, groundColor;
		public float skyIntensity = 1f;
		public float skyRotation = 0;

		public bool autoMode;
		public MyColorSpace colorSpace;

		// Volumetric Light
		public VolumetricLightType vLight;
		public VLightLevel vLightLevel;

		// Fog
		CustomFog vFog;
		float fDistance = 0;
		float fHeight = 30f;
		[Range(0, 1)]
		float fheightDensity = 0.5f;
		Color fColor = Color.white;
		[Range(0, 10)]
		float fogIntensity = 1f;
		float fogIntensityHeight;
		bool isMobile;

		public LightsShadow psShadow;
		public float bakedResolution = 10f;
		public bool helpBox;
		public EnlightenQuality enlightenQuality;

		// Private variabled
		Color redColor;
		bool lightError;
		bool lightChecked;
		GUIStyle myFoldoutStyle;
		bool showLogs;
		// Display window elements (Lighting Box)   
		Vector2 scrollPos = Vector2.zero;

		// Camera
		public Camera mainCamera;
		/*
		// Foliage
		public float translucency;
		public float ambient;
		public float shadows;
		public Color tranColor;
		public float windSpeed;
		public float windScale;
		public string CustomShader = "Legacy Shaders/Transparent/Diffuse";
		*/
		// Snow
		public Texture2D snowAlbedo;
		public Texture2D snowNormal;
		public float snowIntensity = 0;
		public string customShaderSnow = "Legacy Shaders/Diffuse";

		// Material converter
		public MatType matType;
		#endregion

		#region Init()
		// Add menu named "My Window" to the Window menu
		[MenuItem("Window/Lighting Box 2 %E")]
		static void Init()
		{

			// Get existing open window or if none, make a new one:
			////	LB_LightingBox window = (LB_LightingBox)EditorWindow.GetWindow(typeof(LB_LightingBox));
			System.Type inspectorType = System.Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll");
			LB_LightingBox window = (LB_LightingBox)EditorWindow.GetWindow<LB_LightingBox>("Lighting Box 2", true, new System.Type[] { inspectorType });

			window.Show();
			window.autoRepaintOnSceneChange = true;
			window.maxSize = new Vector2(1000f, 1000f);
			window.minSize = new Vector2(387f, 1000f);

		}
		#endregion

		#region Options
		// Internal Usage
		public bool LightingBoxState = true, OptionsState = true;
		public bool ambientState = true;
		public bool sunState = true;
		public bool lightSettingsState = true;
		public bool cameraState = true;
		public bool profileState = true;
		public bool buildState = true;
		public bool vLightState = true;
		public bool sunShaftState = true;
		public bool fogState = true;
		public bool dofState = true;
		public bool autoFocusState = true;
		public bool colorState = true;
		public bool bloomState = true;
		public bool aaState = true;
		public bool aoState = true;
		public bool motionBlurState = true;
		public bool vignetteState = true;
		public bool chromatticState = true;
		public bool ssrState = true;
		public bool st_ssrState;
		public bool foliageState = true;
		public bool snowState = true;

		// Effects enabled
		public bool Ambient_Enabled = true;
		public bool Scene_Enabled = true;
		public bool Sun_Enabled = true;
		public bool VL_Enabled = false;
		public bool SunShaft_Enabled = false;
		public bool Fog_Enabled = false;
		public bool DOF_Enabled = true;
		public bool Bloom_Enabled = false;
		public bool AA_Enabled = true;
		public bool AO_Enabled = false;
		public bool MotionBlur_Enabled = true;
		public bool Vignette_Enabled = true;
		public bool Chromattic_Enabled = true;
		public bool SSR_Enabled = false;
		public bool AutoFocus_Enabled = false;
		public bool ST_SSR_Enabled = false;

		Texture2D arrowOn, arrowOff;

		#endregion

		void NewSceneInit()
		{
			if (EditorSceneManager.GetActiveScene().name == "")
			{
				LB_LightingProfile = Resources.Load("DefaultSettings") as LB_LightingProfile;
				helper.Update_MainProfile(LB_LightingProfile);

				OnLoad();
				currentScene = EditorSceneManager.GetActiveScene().name;

			}
			else
			{
				if (System.String.IsNullOrEmpty(ConfigUtility.GetConfig(EditorSceneManager.GetActiveScene().name)))
				{
					LB_LightingProfile = Resources.Load("DefaultSettings") as LB_LightingProfile;
					helper.Update_MainProfile(LB_LightingProfile);

				}
				else
				{
					LB_LightingProfile = (LB_LightingProfile)AssetDatabase.LoadAssetAtPath(ConfigUtility.GetConfig(EditorSceneManager.GetActiveScene().name), typeof(LB_LightingProfile));
					helper.Update_MainProfile(LB_LightingProfile);

				}

				OnLoad();
				currentScene = EditorSceneManager.GetActiveScene().name;

			}


		}

		// Load and apply default settings when a new scene opened
		void OnNewSceneOpened()
		{
			NewSceneInit();
		}

		void OnDisable()
		{
			EditorApplication.hierarchyChanged -= OnNewSceneOpened;
		}

		void OnEnable()
		{
			arrowOn = Resources.Load("arrowOn") as Texture2D;
			arrowOff = Resources.Load("arrowOff") as Texture2D;

			if (!GameObject.Find("LightingBox_Helper"))
			{
				GameObject helperObject = new GameObject("LightingBox_Helper");
				helperObject.AddComponent<LB_LightingBoxHelper>();
				helper = helperObject.GetComponent<LB_LightingBoxHelper>();
			}

			EditorApplication.hierarchyChanged += OnNewSceneOpened;

			currentScene = EditorSceneManager.GetActiveScene().name;

			if (System.String.IsNullOrEmpty(ConfigUtility.GetConfig(EditorSceneManager.GetActiveScene().name)))
				LB_LightingProfile = Resources.Load("DefaultSettings") as LB_LightingProfile;
			else
				LB_LightingProfile = (LB_LightingProfile)AssetDatabase.LoadAssetAtPath(ConfigUtility.GetConfig(EditorSceneManager.GetActiveScene().name), typeof(LB_LightingProfile));

			OnLoad();

		}

		void OnGUI()
		{

			#region Styles
			GUIStyle redStyle = new GUIStyle(EditorStyles.label);
			redStyle.alignment = TextAnchor.MiddleLeft;
			redStyle.normal.textColor = Color.red;

			GUIStyle blueStyle = new GUIStyle(EditorStyles.label);
			blueStyle.alignment = TextAnchor.MiddleLeft;
			blueStyle.normal.textColor = Color.blue;


			GUIStyle stateButton = new GUIStyle();
			stateButton = "Label";
			stateButton.alignment = TextAnchor.MiddleLeft;
			stateButton.fontStyle = FontStyle.Bold;

			GUIStyle buttonMain = new GUIStyle();
			buttonMain = "Box";
			buttonMain.alignment = TextAnchor.MiddleCenter;
			buttonMain.fontStyle = FontStyle.Bold;

			#endregion

			#region GUI start implementation
			Undo.RecordObject(this, "lb");

			scrollPos = EditorGUILayout.BeginScrollView(scrollPos,
				false,
				false,
				GUILayout.Width(this.position.width),
				GUILayout.Height(this.position.height));

			EditorGUILayout.Space();

			GUILayout.Label("Lighting Box 2 - Unity 2022.3 - Update 2.9.9 Dec 2024", EditorStyles.helpBox);


			EditorGUILayout.BeginHorizontal();

			if (!helpBox)
			{
				if (GUILayout.Button("Show Help", GUILayout.Width(177), GUILayout.Height(24f)))
				{
					helpBox = !helpBox;
				}
			}
			else
			{
				if (GUILayout.Button("Hide Help", GUILayout.Width(177), GUILayout.Height(24f)))
				{
					helpBox = !helpBox;
				}
			}
			if (GUILayout.Button("Refresh", GUILayout.Width(179), GUILayout.Height(24f)))
			{
				UpdateSettings();
				UpdatePostEffects();
			}

			EditorGUILayout.EndHorizontal();


			if (EditorPrefs.GetInt("RateLB") != 3)
			{

				if (GUILayout.Button("Rate Lighting Box"))
				{
					EditorPrefs.SetInt("RateLB", 3);
					Application.OpenURL("http://u3d.as/Se9");
				}
			}

			EditorGUILayout.Space();
			EditorGUILayout.Space();


			#endregion

			#region Tabs
			EditorGUILayout.BeginHorizontal();
			//----------------------------------------------
			if (winMode == WindowMode.Part1)
				GUI.backgroundColor = Color.green;
			else
				GUI.backgroundColor = Color.white;
			//----------------------------------------------
			if (GUILayout.Button("Scene", GUILayout.Width(87), GUILayout.Height(43)))
				winMode = WindowMode.Part1;
			//----------------------------------------------
			if (winMode == WindowMode.Part2)
				GUI.backgroundColor = Color.green;
			else
				GUI.backgroundColor = Color.white;
			//----------------------------------------------
			if (GUILayout.Button("Effect", GUILayout.Width(87), GUILayout.Height(43)))
				winMode = WindowMode.Part2;
			//----------------------------------------------
			if (winMode == WindowMode.Part3)
				GUI.backgroundColor = Color.green;
			else
				GUI.backgroundColor = Color.white;
			//----------------------------------------------
			if (GUILayout.Button("Color", GUILayout.Width(87), GUILayout.Height(43)))
				winMode = WindowMode.Part3;
			//----------------------------------------------
			if (winMode == WindowMode.Finish)
				GUI.backgroundColor = Color.green;
			else
				GUI.backgroundColor = Color.white;
			//----------------------------------------------
			if (GUILayout.Button("Screen", GUILayout.Width(87), GUILayout.Height(43)))
				winMode = WindowMode.Finish;
			//----------------------------------------------
			GUI.backgroundColor = Color.white;
			//----------------------------------------------//----------------------------------------------//----------------------------------------------

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			#endregion

			#region Toolbar

			EditorGUILayout.BeginHorizontal();
			if (LightingBoxState)
			{
				if (GUILayout.Button("Effects On", GUILayout.Width(177), GUILayout.Height(24)))
				{
					helper.Toggle_Effects();

					LightingBoxState = !LightingBoxState;

					if (LB_LightingProfile)
						LB_LightingProfile.LightingBoxState = LightingBoxState;
				}
			}
			else
			{
				if (GUILayout.Button("Effects Off", GUILayout.Width(177), GUILayout.Height(24)))
				{
					helper.Toggle_Effects();
					LightingBoxState = !LightingBoxState;

					if (LB_LightingProfile)
						LB_LightingProfile.LightingBoxState = LightingBoxState;
				}
			}
			if (OptionsState)
			{
				if (GUILayout.Button("Expand All", GUILayout.Width(179), GUILayout.Height(24)))
				{
					ambientState = sunState = lightSettingsState = true;
					cameraState = profileState = buildState = true;
					vLightState = sunShaftState = fogState = true;
					dofState = autoFocusState = colorState = true;
					bloomState = aaState = aoState = true;
					motionBlurState = vignetteState = chromatticState = true;
					ssrState = foliageState = snowState = st_ssrState = true;
					OptionsState = !OptionsState;

					if (LB_LightingProfile)
					{
						LB_LightingProfile.ambientState = ambientState;
						LB_LightingProfile.sunState = sunState;
						LB_LightingProfile.lightSettingsState = lightSettingsState;
						LB_LightingProfile.cameraState = cameraState;
						LB_LightingProfile.profileState = profileState;
						LB_LightingProfile.buildState = buildState;
						LB_LightingProfile.vLightState = vLightState;
						LB_LightingProfile.sunShaftState = sunShaftState;
						LB_LightingProfile.fogState = fogState;
						LB_LightingProfile.dofState = dofState;
						LB_LightingProfile.autoFocusState = autoFocusState;
						LB_LightingProfile.colorState = colorState;
						LB_LightingProfile.bloomState = bloomState;
						LB_LightingProfile.aaState = aaState;
						LB_LightingProfile.aoState = aoState;
						LB_LightingProfile.motionBlurState = motionBlurState;
						LB_LightingProfile.vignetteState = vignetteState;
						LB_LightingProfile.chromatticState = chromatticState;
						LB_LightingProfile.ssrState = ssrState;
						LB_LightingProfile.st_ssrState = st_ssrState;
						LB_LightingProfile.foliageState = foliageState;
						LB_LightingProfile.snowState = snowState;
						LB_LightingProfile.OptionsState = OptionsState;
						EditorUtility.SetDirty(LB_LightingProfile);
					}

				}
			}
			else
			{
				if (GUILayout.Button("Close All", GUILayout.Width(179), GUILayout.Height(24)))
				{

					ambientState = sunState = lightSettingsState = false;
					cameraState = profileState = buildState = false;
					vLightState = sunShaftState = fogState = false;
					dofState = autoFocusState = colorState = false;
					bloomState = aaState = aoState = false;
					motionBlurState = vignetteState = chromatticState = false;
					ssrState = foliageState = snowState = st_ssrState = false;
					OptionsState = !OptionsState;

					if (LB_LightingProfile)
					{
						LB_LightingProfile.ambientState = ambientState;
						LB_LightingProfile.sunState = sunState;
						LB_LightingProfile.lightSettingsState = lightSettingsState;
						LB_LightingProfile.cameraState = cameraState;
						LB_LightingProfile.profileState = profileState;
						LB_LightingProfile.buildState = buildState;
						LB_LightingProfile.vLightState = vLightState;
						LB_LightingProfile.sunShaftState = sunShaftState;
						LB_LightingProfile.fogState = fogState;
						LB_LightingProfile.dofState = dofState;
						LB_LightingProfile.autoFocusState = autoFocusState;
						LB_LightingProfile.colorState = colorState;
						LB_LightingProfile.bloomState = bloomState;
						LB_LightingProfile.aaState = aaState;
						LB_LightingProfile.aoState = aoState;
						LB_LightingProfile.motionBlurState = motionBlurState;
						LB_LightingProfile.vignetteState = vignetteState;
						LB_LightingProfile.chromatticState = chromatticState;
						LB_LightingProfile.ssrState = ssrState;
						LB_LightingProfile.st_ssrState = st_ssrState;
						LB_LightingProfile.foliageState = foliageState;
						LB_LightingProfile.snowState = snowState;
						LB_LightingProfile.OptionsState = OptionsState;
						EditorUtility.SetDirty(LB_LightingProfile);
					}

				}
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();
			EditorGUILayout.Space();
			#endregion

			if (winMode == WindowMode.Part1)
			{

				#region Toggle Settings


				#endregion

				#region Profiles

				//-----------Profile----------------------------------------------------------------------------
				GUILayout.BeginVertical("Box");

				EditorGUILayout.BeginHorizontal();

				if (profileState)
					GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
				else
					GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

				var profileStateRef = profileState;

				if (GUILayout.Button("Profile", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
				{
					profileState = !profileState;
				}

				if (profileStateRef != profileState)
				{

					if (LB_LightingProfile)
					{
						LB_LightingProfile.profileState = profileState;
						EditorUtility.SetDirty(LB_LightingProfile);
					}

				}
				EditorGUILayout.EndHorizontal();

				GUILayout.EndVertical();
				//---------------------------------------------------------------------------------------

				if (profileState)
				{
					if (helpBox)
						EditorGUILayout.HelpBox("Create LB_LightingBox settings profile and Post Processing Stack Profiles for each scene", MessageType.Info);

					var lightingProfileRef = LB_LightingProfile;
					var postProcessingProfileRef = postProcessingProfile;

					EditorGUILayout.BeginHorizontal();
					LB_LightingProfile = EditorGUILayout.ObjectField("Lighting Profile", LB_LightingProfile, typeof(LB_LightingProfile), true) as LB_LightingProfile;

					if (GUILayout.Button("New", GUILayout.Width(43), GUILayout.Height(17)))
					{

						if (EditorSceneManager.GetActiveScene().name == "")
							EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

						string path = EditorUtility.SaveFilePanelInProject("Save As ...", "Lighting_Profile_" + EditorSceneManager.GetActiveScene().name, "asset", "");

						if (path != "")
						{
							LB_LightingProfile = new LB_LightingProfile();

							AssetDatabase.CreateAsset(LB_LightingProfile, path);
							AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(Resources.Load("DefaultSettings_LB")), path);
							LB_LightingProfile = (LB_LightingProfile)AssetDatabase.LoadAssetAtPath(path, typeof(LB_LightingProfile));
							helper.Update_MainProfile(LB_LightingProfile);

							AssetDatabase.Refresh();

							string path2 = System.IO.Path.GetDirectoryName(path) + "/Post_Profile_" + EditorSceneManager.GetActiveScene().name + ".asset";
							// Create new post processing stack 2 profile
							postProcessingProfile = new PostProcessProfile();
							AssetDatabase.CreateAsset(postProcessingProfile, path2);
							AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(Resources.Load("Default_Post_Profile")), path2);
							postProcessingProfile = (PostProcessProfile)AssetDatabase.LoadAssetAtPath(path2, typeof(PostProcessProfile));
							LB_LightingProfile.postProcessingProfile = postProcessingProfile;

							AssetDatabase.Refresh();

						}
					}
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.Space();

					if (lightingProfileRef != LB_LightingProfile)
					{

						helper.Update_MainProfile(LB_LightingProfile);
						OnLoad();
						ConfigUtility.AddConfig(EditorSceneManager.GetActiveScene().name, AssetDatabase.GetAssetPath(LB_LightingProfile));

						if (LB_LightingProfile)
							EditorUtility.SetDirty(LB_LightingProfile);
					}

					if (postProcessingProfileRef != postProcessingProfile)
					{
						if (LB_LightingProfile)
						{
							LB_LightingProfile.postProcessingProfile = postProcessingProfile;
							EditorUtility.SetDirty(LB_LightingProfile);
						}

						UpdatePostEffects();

					}




					if (helpBox)
						EditorGUILayout.HelpBox("Which camera should has effects", MessageType.Info);

					EditorGUILayout.BeginHorizontal();
					var mainCameraRef = mainCamera;

					mainCamera = EditorGUILayout.ObjectField("Target Camera", mainCamera, typeof(Camera), true) as Camera;
					if (GUILayout.Button("Save", GUILayout.Width(43), GUILayout.Height(17)))
					{
						if (LB_LightingProfile)
						{
							LB_LightingProfile.mainCameraName = mainCamera.name;
							EditorUtility.SetDirty(LB_LightingProfile);
						}
					}
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.Space();

					if (mainCameraRef != mainCamera)
					{
						UpdatePostEffects();
						UpdateSettings();

						if (LB_LightingProfile)
						{
							LB_LightingProfile.mainCameraName = mainCamera.name;
							EditorUtility.SetDirty(LB_LightingProfile);
						}
					}


					#region Auto Profile

					if (LB_LightingProfile && LB_LightingProfile.name != "DefaultSettings" &&
						 LB_LightingProfile.name != "DefaultSettings_LB")
					{
						EditorGUILayout.BeginHorizontal();

						autoProfile = (AutoProfile)EditorGUILayout.EnumPopup("Reset Profile", autoProfile, GUILayout.Width(310));
						if (GUILayout.Button("Apply", GUILayout.Width(43), GUILayout.Height(17)))
						{
							if (autoProfile == AutoProfile.Default)
							{
								AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath
									(Resources.Load("Config/Default")), ConfigUtility.GetConfig(EditorSceneManager.GetActiveScene().name));

								AssetDatabase.Refresh();

								helper.Update_MainProfile(LB_LightingProfile);
								OnLoad();

								if (LB_LightingProfile)
									EditorUtility.SetDirty(LB_LightingProfile);
							}
						}
						EditorGUILayout.EndHorizontal();
					}

					#endregion

					EditorGUILayout.Space();

					#region Target Platform

					var targetPlatformRef = targetPlatform;

					if (helpBox)
						EditorGUILayout.HelpBox("Choose target platform", MessageType.Info);

					EditorGUILayout.BeginHorizontal();

					targetPlatform = (Target_Platform)EditorGUILayout.EnumPopup("Target Platform", targetPlatform, GUILayout.Width(343));

					EditorGUILayout.EndHorizontal();

					if (targetPlatformRef != targetPlatform)
					{

						if (targetPlatform == Target_Platform.Mobile)
							isMobile = true;
						else
							isMobile = false;

						if (LB_LightingProfile)
						{
							if (targetPlatform == Target_Platform.Mobile)
							{
								renderPath = Render_Path.Forward;
								mobileOptimizedBloom = true;
								LB_LightingProfile.renderPath = renderPath;
								helper.Update_RenderPath(true, Render_Path.Forward, mainCamera);
							}
							else
							{
								renderPath = Render_Path.Deferred;
								LB_LightingProfile.renderPath = renderPath;
								mobileOptimizedBloom = false;
								helper.Update_RenderPath(true, Render_Path.Deferred, mainCamera);
							}

							LB_LightingProfile.targetPlatform = targetPlatform;

							if (targetPlatform == Target_Platform.Mobile)
								isMobile = true;
							else
								isMobile = false;

							LB_LightingProfile.isMobile = isMobile;
							LB_LightingProfile.mobileOptimizedBloom = mobileOptimizedBloom;

							EditorUtility.SetDirty(LB_LightingProfile);
						}

						if (targetPlatform == Target_Platform.Mobile)
						{
							// helper.Update_GlobalFog(mainCamera, false, vFog, fDistance, fHeight, fheightDensity, fColor, fogIntensity, isMobile);
							helper.Update_AO(mainCamera, false, aoType, aoRadius, aoIntensity, ambientOnly, aoColor, aoQuality);
							helper.Update_StochasticSSR(mainCamera, false, resolutionMode, debugPass, rayDistance, screenFadeSize, smoothnessRange);
							helper.Update_SSR(mainCamera, false, ssrQuality, ssrAtten, ssrFade);
							helper.Update_VolumetricLight(mainCamera, false, vLight, vLightLevel);
							helper.Update_RenderPath(Scene_Enabled, Render_Path.Forward, mainCamera);
							helper.Update_DOF(DOF_Enabled, focusDistance, AperTure, fLength, kernelSize);
							helper.Update_ColorGrading(colorMode, exposureIntensity, contrastValue, temp, exposureCompensation, saturation, colorGamma, colorLift, gamma, lut, exposureMode, eyeMin, eyeMax, targetPlatform);
						}
						else
						{
							OnLoad();
						}
					}
					#endregion

					EditorGUILayout.Space();
					EditorGUILayout.Space();

				}

				#endregion

				#region Ambient

				//-----------Ambient----------------------------------------------------------------------------
				GUILayout.BeginVertical("Box");

				EditorGUILayout.BeginHorizontal();

				if (ambientState)
					GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
				else
					GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

				var Ambient_EnabledRef = Ambient_Enabled;
				var ambientStateRef = ambientState;

				if (GUILayout.Button("Ambient", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
				{
					ambientState = !ambientState;
				}

				if (ambientStateRef != ambientState)
				{
					if (LB_LightingProfile)
					{
						LB_LightingProfile.ambientState = ambientState;
						EditorUtility.SetDirty(LB_LightingProfile);
					}
				}

				EditorGUILayout.EndHorizontal();

				GUILayout.EndVertical();
				//---------------------------------------------------------------------------------------


				if (ambientState)
				{

					if (helpBox)
					{
						EditorGUILayout.HelpBox("Assign scene skybox material here ", MessageType.Info);
					}
					var skyboxRef = skyBox;
					var skyIntensityRef = skyIntensity;
					var skyCubeRef = skyCube;
					var skyBoxExposureRef = skyBoxExposure;
					var skyRotationRef = skyRotation;

					Ambient_Enabled = EditorGUILayout.Toggle("Enabled", Ambient_Enabled);
					EditorGUILayout.Space();

					if (skyBox)
					{
						if (skyBox.shader != Shader.Find("Skybox/Cubemap") && skyBox.name != "Default-Skybox")
						{
							if (GUILayout.Button("Convert to HDRI"))
								skyBox.shader = Shader.Find("Skybox/Cubemap");
						}
					}
					skyBox = EditorGUILayout.ObjectField("SkyBox Material", skyBox, typeof(Material), true) as Material;

					skyBoxExposure = EditorGUILayout.Slider("Sky Exposure", skyBoxExposure, 0.1f, 2f);

					skyRotation = EditorGUILayout.Slider("Rotation", skyRotation, 0, 360f);

					EditorGUILayout.Space();

					if (skyBox)
					{
						if (skyBox.shader == Shader.Find("Skybox/Cubemap"))
						{
							skyCube = EditorGUILayout.ObjectField("HDRI Cubemap", skyCube, typeof(Cubemap), true) as Cubemap;
							EditorGUILayout.Space();

						}
					}

					if (skyboxRef != skyBox || skyRotationRef != skyRotation || skyBoxExposureRef != skyBoxExposure || skyIntensityRef != skyIntensity || skyCubeRef != skyCube)
					{

						helper.Update_SkyBox(Ambient_Enabled, skyBox, skyIntensity, skyCube, skyBoxExposure, skyRotation);

						if (LB_LightingProfile)
						{
							LB_LightingProfile.skyBox = skyBox;
							LB_LightingProfile.skyCube = skyCube;
							LB_LightingProfile.skyBoxExposure = skyBoxExposure;
							LB_LightingProfile.skyRotation = skyRotation;
							EditorUtility.SetDirty(LB_LightingProfile);
						}
					}


					if (helpBox)
						EditorGUILayout.HelpBox("Set ambient lighting source as Skybox(IBL) or a simple color", MessageType.Info);

					var ambientLightRef = ambientLight;
					var ambientColorRef = ambientColor;
					var skyBoxRef = skyBox;
					var skyColorRef = skyColor;
					var equatorColorRef = equatorColor;
					var groundColorRef = groundColor;

					// choose ambient lighting mode   (color or skybox)
					ambientLight = (AmbientLight)EditorGUILayout.EnumPopup("Ambient Source", ambientLight, GUILayout.Width(343));

					if (ambientLight == AmbientLight.Skybox)
						skyIntensity = EditorGUILayout.Slider("Sky Intensity", skyIntensity, 0, 5);

					if (ambientLight == AmbientLight.Color)
						ambientColor = EditorGUILayout.ColorField("Color", ambientColor);
					if (ambientLight == AmbientLight.Gradient)
					{
						skyColor = EditorGUILayout.ColorField("Sky Color", skyColor);
						equatorColor = EditorGUILayout.ColorField("Equator Color", equatorColor);
						groundColor = EditorGUILayout.ColorField("Ground Color", groundColor);
					}

					if (ambientLightRef != ambientLight || ambientColorRef != ambientColor
						|| skyBoxRef != skyBox || skyCubeRef != skyCube || skyColorRef != skyColor || skyIntensityRef != skyIntensity
						|| equatorColorRef != equatorColor || groundColorRef != groundColor
						|| Ambient_EnabledRef != Ambient_Enabled)
					{
						helper.Update_Ambient(Ambient_Enabled, ambientLight, ambientColor, skyColor, equatorColor, groundColor);
						helper.Update_SkyBox(Ambient_Enabled, skyBox, skyIntensity, skyCube, skyBoxExposure, skyRotation);

						if (LB_LightingProfile)
						{
							LB_LightingProfile.ambientColor = ambientColor;
							LB_LightingProfile.ambientLight = ambientLight;
							LB_LightingProfile.skyBox = skyBox;
							LB_LightingProfile.skyCube = skyCube;
							LB_LightingProfile.skyBoxExposure = skyBoxExposure;
							LB_LightingProfile.skyIntensity = skyIntensity;
							LB_LightingProfile.skyColor = skyColor;
							LB_LightingProfile.equatorColor = equatorColor;
							LB_LightingProfile.groundColor = groundColor;
							LB_LightingProfile.Ambient_Enabled = Ambient_Enabled;
							EditorUtility.SetDirty(LB_LightingProfile);
						}
					}
					//----------------------------------------------------------------------
				}
				#endregion

				#region Sun Light
				//-----------Sun----------------------------------------------------------------------------
				GUILayout.BeginVertical("Box");

				EditorGUILayout.BeginHorizontal();

				if (sunState)
					GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
				else
					GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

				var sunStateRef = sunState;

				if (GUILayout.Button("Sun", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
				{
					sunState = !sunState;
				}

				if (sunStateRef != sunState)
				{
					if (LB_LightingProfile)
					{
						LB_LightingProfile.sunState = sunState;
						EditorUtility.SetDirty(LB_LightingProfile);
					}
				}

				EditorGUILayout.EndHorizontal();

				GUILayout.EndVertical();
				//---------------------------------------------------------------------------------------


				if (sunState)
				{

					if (helpBox)
						EditorGUILayout.HelpBox("Sun / Moon light settings", MessageType.Info);

					var Sun_EnabledRef = Sun_Enabled;

					Sun_Enabled = EditorGUILayout.Toggle("Enabled", Sun_Enabled);

					EditorGUILayout.Space();

					EditorGUILayout.BeginHorizontal();
					sunLight = EditorGUILayout.ObjectField("Sun Light", sunLight, typeof(Light), true) as Light;
					if (!sunLight)
					{
						if (GUILayout.Button("Find"))
							Update_Sun();
					}
					EditorGUILayout.EndHorizontal();
					var sunColorRef = sunColor;

					sunColor = EditorGUILayout.ColorField("Color", sunColor);

					var sunIntensityRef = sunIntensity;
					var indirectIntensityRef = indirectIntensity;

					sunIntensity = EditorGUILayout.Slider("Intenity", sunIntensity, 0, 4f);
					indirectIntensity = EditorGUILayout.Slider("Indirect Intensity", indirectIntensity, 0, 4f);

					var sunFlareRef = sunFlare;

					sunFlare = EditorGUILayout.ObjectField("Lens Flare", sunFlare, typeof(Flare), true) as Flare;

					if (Sun_EnabledRef != Sun_Enabled)
					{
						if (LB_LightingProfile)
						{
							LB_LightingProfile.Sun_Enabled = Sun_Enabled;
							EditorUtility.SetDirty(LB_LightingProfile);
						}
					}

					if (Sun_Enabled)
					{

						if (sunColorRef != sunColor || Sun_EnabledRef != Sun_Enabled)
						{

							if (sunLight)
								sunLight.color = sunColor;
							else
								Update_Sun();
							if (LB_LightingProfile)
							{
								LB_LightingProfile.sunColor = sunColor;
								EditorUtility.SetDirty(LB_LightingProfile);
							}
						}

						if (sunIntensityRef != sunIntensity || indirectIntensityRef != indirectIntensity
							|| Sun_EnabledRef != Sun_Enabled)
						{

							if (sunLight)
							{
								sunLight.intensity = sunIntensity;
								sunLight.bounceIntensity = indirectIntensity;
							}
							else
								Update_Sun();
							if (LB_LightingProfile)
							{
								LB_LightingProfile.sunIntensity = sunIntensity;
								LB_LightingProfile.indirectIntensity = indirectIntensity;
								LB_LightingProfile.Sun_Enabled = Sun_Enabled;
							}
							if (LB_LightingProfile)
							{
								LB_LightingProfile.sunState = sunState;
								EditorUtility.SetDirty(LB_LightingProfile);
							}
						}
						if (sunFlareRef != sunFlare)
						{
							if (sunFlare)
							{
								if (sunLight)
									sunLight.flare = sunFlare;
							}
							else
							{
								if (sunLight)
									sunLight.flare = null;
							}

							if (LB_LightingProfile)
							{
								LB_LightingProfile.sunFlare = sunFlare;
								LB_LightingProfile.sunState = sunState;
								EditorUtility.SetDirty(LB_LightingProfile);
							}
						}
					}
				}
				#endregion

				#region Lighting Mode


				//-----------Light Settings----------------------------------------------------------------------------
				GUILayout.BeginVertical("Box");

				EditorGUILayout.BeginHorizontal();

				if (lightSettingsState)
					GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
				else
					GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

				var lightSettingsStateRef = lightSettingsState;
				var Scene_EnabledRef = Scene_Enabled;

				if (GUILayout.Button("Scene", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
				{
					lightSettingsState = !lightSettingsState;
				}

				if (lightSettingsStateRef != lightSettingsState)
				{
					if (LB_LightingProfile)
					{
						LB_LightingProfile.lightSettingsState = lightSettingsState;
						LB_LightingProfile.Scene_Enabled = Scene_Enabled;
						EditorUtility.SetDirty(LB_LightingProfile);
					}
				}

				EditorGUILayout.EndHorizontal();

				GUILayout.EndVertical();
				//---------------------------------------------------------------------------------------


				if (lightSettingsState)
				{

					if (helpBox)
						EditorGUILayout.HelpBox("Fully realtime without GI, Enlighten Realtime GI or Baked Progressive Lightmapper", MessageType.Info);

					var lightingModeRef = lightingMode;
					var enlightenQualityRef = enlightenQuality;
					var bakedResolutionRef = bakedResolution;

					Scene_Enabled = EditorGUILayout.Toggle("Enabled", Scene_Enabled);
					EditorGUILayout.Space();

					// Choose lighting mode (realtime GI or baked GI)
					lightingMode = (LightingMode)EditorGUILayout.EnumPopup("Lighting Mode", lightingMode, GUILayout.Width(343));


					EditorGUILayout.Space();

					if (helpBox)
						EditorGUILayout.HelpBox("Baked lightmapping resolution. Higher value needs more RAM and longer bake time. Check task manager about RAM usage during bake time", MessageType.Info);

					// Baked lightmapping resolution   

					if (lightingMode == LightingMode.BakedCPU ||
					lightingMode == LightingMode.BakedGPU ||
					lightingMode == LightingMode.BakedEnlighten)
						bakedResolution = EditorGUILayout.FloatField("Baked Resolution", bakedResolution);

					if (lightingMode != LightingMode.FullyRealtime)
						EditorGUILayout.Space();

					if (lightingMode == LightingMode.BakedEnlighten || lightingMode == LightingMode.RealtimeGI || lightingMode == LightingMode.RealtimeGiAndBakedGI)
						enlightenQuality = (EnlightenQuality)EditorGUILayout.EnumPopup("Enlighten Quality", enlightenQuality, GUILayout.Width(343));
					else
						enlightenQuality = EnlightenQuality.High;
					try
					{
						Lightmapping.lightingSettings.lightmapResolution = bakedResolution;
					}
					catch { }

					if (enlightenQualityRef != enlightenQuality ||
						bakedResolutionRef != bakedResolution)
					{

						#region Very Low
						LightmapParameters very_low = new LightmapParameters();

						very_low.name = "Lighting Box Very-Low";
						very_low.resolution = 0.125f;
						very_low.clusterResolution = 0.4f;
						very_low.irradianceBudget = 96;
						very_low.irradianceQuality = 8192;
						very_low.modellingTolerance = 0.001f;
						very_low.stitchEdges = true;
						very_low.isTransparent = false;
						very_low.systemTag = -1;
						very_low.blurRadius = 2;
						very_low.antiAliasingSamples = 8;
						very_low.directLightQuality = 64;
						very_low.bakedLightmapTag = -1;
						very_low.AOQuality = 256;
						very_low.AOAntiAliasingSamples = 16;
						very_low.backFaceTolerance = 0.7f;
						#endregion

						#region Low
						LightmapParameters low = new LightmapParameters();

						low.name = "Lighting Box Low";
						low.resolution = 0.5f;
						low.clusterResolution = 0.4f;
						low.irradianceBudget = 96;
						low.irradianceQuality = 8192;
						low.modellingTolerance = 0.001f;
						low.stitchEdges = true;
						low.isTransparent = false;
						low.systemTag = -1;
						low.blurRadius = 2;
						low.antiAliasingSamples = 8;
						low.directLightQuality = 64;
						low.bakedLightmapTag = -1;
						low.AOQuality = 256;
						low.AOAntiAliasingSamples = 16;
						low.backFaceTolerance = 0.7f;
						#endregion

						#region Medium
						LightmapParameters medium = new LightmapParameters();

						medium.name = "Lighting Box Medium";
						medium.resolution = 1f;
						medium.clusterResolution = 0.5f;
						medium.irradianceBudget = 128;
						medium.irradianceQuality = 8192;
						medium.modellingTolerance = 0.01f;
						medium.stitchEdges = true;
						medium.isTransparent = false;
						medium.systemTag = -1;
						medium.blurRadius = 2;
						medium.antiAliasingSamples = 8;
						medium.directLightQuality = 64;
						medium.bakedLightmapTag = -1;
						medium.AOQuality = 256;
						medium.AOAntiAliasingSamples = 16;
						medium.backFaceTolerance = 0.7f;
						#endregion

						#region High
						LightmapParameters high = new LightmapParameters();

						high.name = "Lighting Box High";
						high.resolution = 2;
						high.clusterResolution = 0.6f;
						high.irradianceBudget = 128;
						high.irradianceQuality = 16384;
						high.modellingTolerance = 0.001f;
						high.stitchEdges = true;
						high.isTransparent = false;
						high.systemTag = -1;
						high.blurRadius = 2;
						high.antiAliasingSamples = 8;
						high.directLightQuality = 64;
						high.bakedLightmapTag = -1;
						high.AOQuality = 256;
						high.AOAntiAliasingSamples = 16;
						high.backFaceTolerance = 0.7f;
						#endregion

						if (enlightenQuality == EnlightenQuality.VeryLow)
							LightmapParameters.SetLightmapParametersForLightingSettings(very_low, Lightmapping.lightingSettings);
						if (enlightenQuality == EnlightenQuality.Low)
							LightmapParameters.SetLightmapParametersForLightingSettings(low, Lightmapping.lightingSettings);
						if (enlightenQuality == EnlightenQuality.Medium)
							LightmapParameters.SetLightmapParametersForLightingSettings(medium, Lightmapping.lightingSettings);
						if (enlightenQuality == EnlightenQuality.High)
							LightmapParameters.SetLightmapParametersForLightingSettings(high, Lightmapping.lightingSettings);

						if (LB_LightingProfile)
						{
							LB_LightingProfile.bakedResolution = bakedResolution;
							LB_LightingProfile.enlightenQuality = enlightenQuality;
							LB_LightingProfile.Scene_Enabled = Scene_Enabled;
							EditorUtility.SetDirty(LB_LightingProfile);
						}
					}

					if (targetPlatform == Target_Platform.WebGL_2)
					{
						if (lightingMode == LightingMode.RealtimeGI)
						{
							EditorGUILayout.Space();
							EditorGUILayout.LabelField("Enlighten Realtime GI is not available for WebGL platform", redStyle);
							EditorGUILayout.Space();

						}
					}
					if (targetPlatform == Target_Platform.Mobile)
					{
						if (lightingMode == LightingMode.RealtimeGI)
						{
							EditorGUILayout.Space();
							EditorGUILayout.LabelField("Enlighten's Realtime GI is not suitable for mobile platform", redStyle);
							EditorGUILayout.Space();

						}
					}
					if (lightingModeRef != lightingMode || Scene_EnabledRef != Scene_Enabled)
					{
						//----------------------------------------------------------------------
						// Update Lighting Mode
						helper.Update_LightingMode(Scene_Enabled, lightingMode);
						//----------------------------------------------------------------------
						if (LB_LightingProfile)
						{
							LB_LightingProfile.lightingMode = lightingMode;
							LB_LightingProfile.Scene_Enabled = Scene_Enabled;
							EditorUtility.SetDirty(LB_LightingProfile);
						}
					}
					#endregion

					#region Color Space
					EditorGUILayout.Space();

					if (helpBox)
						EditorGUILayout.HelpBox("Choose between Linear or Gamma color space , default should be Linear for my settings and next-gen platfroms", MessageType.Info);

					var colorSpaceRef = colorSpace;

					// Choose color space (Linear,Gamma) i have used Linear in post effect setting s
					colorSpace = (MyColorSpace)EditorGUILayout.EnumPopup("Color Space", colorSpace, GUILayout.Width(343));

					if (colorSpaceRef != colorSpace)
					{
						// Color Space
						helper.Update_ColorSpace(Scene_Enabled, colorSpace);

						if (LB_LightingProfile)
						{
							LB_LightingProfile.colorSpace = colorSpace;
							EditorUtility.SetDirty(LB_LightingProfile);
						}
					}
					#endregion

					#region Render Path
					EditorGUILayout.Space();

					if (helpBox)
						EditorGUILayout.HelpBox("Choose between Forward and Deferred rendering path for scene cameras. Deferred needed for Screen Space Reflection effect. Forward has better performance in unity", MessageType.Info);

					var renderPathRef = renderPath;

					renderPath = (Render_Path)EditorGUILayout.EnumPopup("Render Path", renderPath, GUILayout.Width(343));

					if (renderPathRef != renderPath)
					{


						helper.Update_RenderPath(Scene_Enabled, renderPath, mainCamera);

						if (LB_LightingProfile)
						{
							LB_LightingProfile.renderPath = renderPath;
							EditorUtility.SetDirty(LB_LightingProfile);
						}

					}

					#endregion

					#region Light Types
					EditorGUILayout.Space();

					if (helpBox)
						EditorGUILayout.HelpBox("Change the type of all light sources (Realtime,Baked,Mixed)", MessageType.Info);

					var lightSettingsRef = lightSettings;

					// Change file lightmapping type mixed,realtime baked
					lightSettings = (LightSettings)EditorGUILayout.EnumPopup("Lights Type", lightSettings, GUILayout.Width(343));

					//----------------------------------------------------------------------
					// Light Types
					if (lightSettingsRef != lightSettings)
					{

						helper.Update_LightSettings(Scene_Enabled, lightSettings);

						if (LB_LightingProfile)
						{
							LB_LightingProfile.lightSettings = lightSettings;
							EditorUtility.SetDirty(LB_LightingProfile);
						}
					}
					//----------------------------------------------------------------------
					#endregion

					#region Light Shadows Settings
					EditorGUILayout.Space();

					if (helpBox)
						EditorGUILayout.HelpBox("Activate / Deactivate shadows for point , spot and directional lights", MessageType.Info);

					var pshadRef = psShadow;
					// Choose hard shadows state on off for spot and point lights
					psShadow = (LightsShadow)EditorGUILayout.EnumPopup("Shadows", psShadow, GUILayout.Width(343));

					if (pshadRef != psShadow)
					{

						// Shadows
						helper.Update_Shadows(Scene_Enabled, psShadow);

						//----------------------------------------------------------------------
						if (LB_LightingProfile)
						{
							LB_LightingProfile.lightsShadow = psShadow;
							EditorUtility.SetDirty(LB_LightingProfile);
						}
					}
					#endregion

					#region Light Probes
					EditorGUILayout.Space();

					if (helpBox)
						EditorGUILayout.HelpBox("Adjust light probes settings for non-static objects, Blend mode is more optimized", MessageType.Info);

					var lightprobeModeRef = lightprobeMode;

					lightprobeMode = (LightProbeMode)EditorGUILayout.EnumPopup("Light Probes", lightprobeMode, GUILayout.Width(343));

					if (targetPlatform == Target_Platform.Mobile)
					{
						if (lightprobeMode == LightProbeMode.Proxy)
							EditorGUILayout.HelpBox("Proxy mode is not suitable for mobile", MessageType.Error);
					}

					if (lightprobeModeRef != lightprobeMode)
					{

						// Light Probes
						helper.Update_LightProbes(Scene_Enabled, lightprobeMode);

						//----------------------------------------------------------------------
						if (LB_LightingProfile)
						{
							LB_LightingProfile.lightProbesMode = lightprobeMode;
							EditorUtility.SetDirty(LB_LightingProfile);
						}
					}
				}
				#endregion

				#region Buttons
				//-----------Buttons----------------------------------------------------------------------------
				GUILayout.BeginVertical("Box");

				EditorGUILayout.BeginHorizontal();

				if (buildState)
					GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
				else
					GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

				var buildStateRef = buildState;

				if (GUILayout.Button("Build", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
				{
					buildState = !buildState;
				}

				if (buildStateRef != buildState)
				{
					if (LB_LightingProfile)
						LB_LightingProfile.buildState = buildState;
					if (LB_LightingProfile)
						EditorUtility.SetDirty(LB_LightingProfile);
				}

				EditorGUILayout.EndHorizontal();

				GUILayout.EndVertical();
				//---------------------------------------------------------------------------------------

				if (buildState)
				{
					var automodeRef = autoMode;

					if (helpBox)
						EditorGUILayout.HelpBox("Automatic lightmap baking", MessageType.Info);


					autoMode = EditorGUILayout.Toggle("Auto Lightmap Mode", autoMode);

					if (automodeRef != autoMode)
					{
						// Auto Mode
						if (autoMode)
							Lightmapping.giWorkflowMode = Lightmapping.GIWorkflowMode.Iterative;
						else
							Lightmapping.giWorkflowMode = Lightmapping.GIWorkflowMode.OnDemand;
						//----------------------------------------------------------------------
						if (LB_LightingProfile)
						{
							LB_LightingProfile.automaticLightmap = autoMode;
							EditorUtility.SetDirty(LB_LightingProfile);
						}
					}

					// Start bake
					if (!Lightmapping.isRunning)
					{

						if (helpBox)
							EditorGUILayout.HelpBox("Bake lightmap", MessageType.Info);

						if (GUILayout.Button("Bake"))
						{
							if (!Lightmapping.isRunning)
							{
								Lightmapping.BakeAsync();
							}
						}

						if (helpBox)
							EditorGUILayout.HelpBox("Clear lightmap data , use lighting window to clean properly", MessageType.Info);

						if (GUILayout.Button("Clear"))
						{
							Lightmapping.Clear();
						}
					}
					else
					{

						if (helpBox)
							EditorGUILayout.HelpBox("Cancel lightmap baking", MessageType.Info);

						if (GUILayout.Button("Cancel"))
						{
							if (Lightmapping.isRunning)
							{
								Lightmapping.Cancel();
							}
						}
					}

					if (Input.GetKey(KeyCode.F))
					{
						if (Lightmapping.isRunning)
							Lightmapping.Cancel();
					}
					if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.E))
					{
						if (!Lightmapping.isRunning)
							Lightmapping.BakeAsync();
					}

					if (helpBox)
					{
						EditorGUILayout.HelpBox("First right click on the scene view, then :", MessageType.Info);

						EditorGUILayout.HelpBox("Bake : Shift + B", MessageType.Info);
						EditorGUILayout.HelpBox("Cancel : Shift + C", MessageType.Info);
						EditorGUILayout.HelpBox("Clear : Shift + E", MessageType.Info);

					}
					EditorGUILayout.Space();
					EditorGUILayout.Space();
					EditorGUILayout.Space();
					EditorGUILayout.Space();

					if (helpBox)
						EditorGUILayout.HelpBox("Open unity Lighting Settings window", MessageType.Info);

					if (GUILayout.Button("Lighting Window"))
					{

						EditorApplication.ExecuteMenuItem("Window/Rendering/Lighting");
					}

					EditorGUILayout.Space();
					EditorGUILayout.Space();

					if (GUILayout.Button("Add Camera Move Script"))
					{
						if (!mainCamera.GetComponent<LB_CameraMove>())
							mainCamera.gameObject.AddComponent<LB_CameraMove>();
					}
					EditorGUILayout.Space();
					EditorGUILayout.Space();
					EditorGUILayout.Space();
					EditorGUILayout.Space();
					EditorGUILayout.Space();
					EditorGUILayout.Space();
					EditorGUILayout.Space();

					#endregion

				}
			}

			if (winMode == WindowMode.Part2)
			{

				if (targetPlatform == Target_Platform.FullFeatured)
				{
					#region Volumetric Light

					//-----------Volumetric Lighting----------------------------------------------------------------------------
					GUILayout.BeginVertical("Box");

					var VL_EnabledRef = VL_Enabled;

					EditorGUILayout.BeginHorizontal();

					if (vLightState)
						GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
					else
						GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

					VL_Enabled = EditorGUILayout.Toggle("", VL_Enabled, GUILayout.Width(30f), GUILayout.Height(17f));

					var vLightStateRef = vLightState;

					if (GUILayout.Button("Volumetric Lighting", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
					{
						vLightState = !vLightState;
					}

					if (vLightStateRef != vLightState)
					{
						if (LB_LightingProfile)
						{
							LB_LightingProfile.vLightState = vLightState;
							EditorUtility.SetDirty(LB_LightingProfile);
						}
					}

					EditorGUILayout.EndHorizontal();

					GUILayout.EndVertical();
					//---------------------------------------------------------------------------------------


					if (vLightState)
					{

						if (helpBox)
							EditorGUILayout.HelpBox("Activate Volumetric Lights For All Light Sources", MessageType.Info);
					}
					var vLightRef = vLight;
					var vLightLevelRef = vLightLevel;

					if (vLightState)
					{
						// Activate or deactivate volumetric lighting for all light sources
						vLight = (VolumetricLightType)EditorGUILayout.EnumPopup("Volumetric Light", vLight, GUILayout.Width(343));


						vLightLevel = (VLightLevel)EditorGUILayout.EnumPopup("Intensity", vLightLevel, GUILayout.Width(343));
					}
					if (vLightRef != vLight || vLightLevelRef != vLightLevel || VL_EnabledRef != VL_Enabled)
					{

						// Volumetric Light
						helper.Update_VolumetricLight(mainCamera, VL_Enabled, vLight, vLightLevel);
						//----------------------------------------------------------------------
						if (LB_LightingProfile)
						{
							LB_LightingProfile.vLight = vLight;
							LB_LightingProfile.vLightLevel = vLightLevel;
							LB_LightingProfile.VL_Enabled = VL_Enabled;
							EditorUtility.SetDirty(LB_LightingProfile);
						}
					}

					#endregion
				}
				else
				{
					VL_Enabled = false;
					helper.Update_VolumetricLight(mainCamera, VL_Enabled, vLight, vLightLevel);
				}

				#region Sun Shaft

				//-----------Sun Shaft----------------------------------------------------------------------------
				GUILayout.BeginVertical("Box");

				var SunShaft_EnabledRef = SunShaft_Enabled;

				EditorGUILayout.BeginHorizontal();

				if (sunShaftState)
					GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
				else
					GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

				SunShaft_Enabled = EditorGUILayout.Toggle("", SunShaft_Enabled, GUILayout.Width(30f), GUILayout.Height(17f));

				var sunShaftStateRef = sunShaftState;

				if (GUILayout.Button("Sun Shaft", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
				{
					sunShaftState = !sunShaftState;
				}

				if (sunShaftStateRef != sunShaftState)
				{
					if (LB_LightingProfile)
						LB_LightingProfile.sunShaftState = sunShaftState;
					if (LB_LightingProfile)
						EditorUtility.SetDirty(LB_LightingProfile);
				}

				EditorGUILayout.EndHorizontal();

				GUILayout.EndVertical();
				//---------------------------------------------------------------------------------------

				if (sunShaftState)
				{
					if (helpBox)
						EditorGUILayout.HelpBox("Activate Sun Shaft for sun", MessageType.Info);

				}
				var shaftDistanceRef = shaftDistance;
				var shaftBlurRef = shaftBlur;
				var shaftColorRef = shaftColor;
				var shaftQualityRef = shaftQuality;

				// Activate Sun Shaft for sun
				///	shaftMode = (ShaftMode)EditorGUILayout.EnumPopup("Sun Shaft",shaftMode,GUILayout.Width(343));


				if (sunShaftState)
				{
					shaftQuality = (SunShafts.SunShaftsResolution)EditorGUILayout.EnumPopup("Quality", shaftQuality, GUILayout.Width(343));
					shaftDistance = 1.0f - EditorGUILayout.Slider("Distance falloff", 1.0f - shaftDistance, 0.1f, 1.0f);
					shaftBlur = EditorGUILayout.Slider("Blur", shaftBlur, 1f, 10f);
					shaftColor = (Color)EditorGUILayout.ColorField("Color", shaftColor);
				}

				if (SunShaft_EnabledRef != SunShaft_Enabled || shaftDistanceRef != shaftDistance
					  || shaftBlurRef != shaftBlur || shaftColorRef != shaftColor || shaftQualityRef != shaftQuality)
				{

					// Sun Shaft
					if (sunLight)
						helper.Update_SunShaft(mainCamera, SunShaft_Enabled, shaftQuality, shaftDistance, shaftBlur, shaftColor, sunLight.transform);
					else
					{
						if (!RenderSettings.sun)
						{
							Light[] lights = GameObject.FindObjectsOfType<Light>();
							foreach (Light l in lights)
							{
								if (l.type == LightType.Directional)
									sunLight = l;
							}
						}
						else
							sunLight = RenderSettings.sun;

						helper.Update_SunShaft(mainCamera, SunShaft_Enabled, shaftQuality, shaftDistance, shaftBlur, shaftColor, sunLight.transform);
					}
					//----------------------------------------------------------------------
					if (LB_LightingProfile)
					{
						LB_LightingProfile.SunShaft_Enabled = SunShaft_Enabled;
						LB_LightingProfile.shaftQuality = shaftQuality;
						LB_LightingProfile.shaftDistance = shaftDistance;
						LB_LightingProfile.shaftBlur = shaftBlur;
						LB_LightingProfile.shaftColor = shaftColor;
						EditorUtility.SetDirty(LB_LightingProfile);
					}
				}

				#endregion


				#region Global Fog


				//-----------Global Fog----------------------------------------------------------------------------
				GUILayout.BeginVertical("Box");

				var Fog_EnabledRef = Fog_Enabled;

				EditorGUILayout.BeginHorizontal();

				if (fogState)
					GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
				else
					GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

				Fog_Enabled = EditorGUILayout.Toggle("", Fog_Enabled, GUILayout.Width(30f), GUILayout.Height(17f));

				var fogStateRef = fogState;

				if (GUILayout.Button("Global Fog", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
				{
					fogState = !fogState;
				}

				if (fogStateRef != fogState)
				{
					if (LB_LightingProfile)
						LB_LightingProfile.fogState = fogState;
					if (LB_LightingProfile)
						EditorUtility.SetDirty(LB_LightingProfile);
				}
				EditorGUILayout.EndHorizontal();

				GUILayout.EndVertical();
				//---------------------------------------------------------------------------------------

				if (fogState)
				{
					if (helpBox)
						EditorGUILayout.HelpBox("Activate Global Fog for the scene. Combined with unity Lighting Window Fog parameters", MessageType.Info);

				}

				var vfogRef = vFog;

				if (fogState)
				{
					if (targetPlatform == Target_Platform.Mobile)
						vFog = CustomFog.Global;
					else
						vFog = (CustomFog)EditorGUILayout.EnumPopup("Global Fog", vFog, GUILayout.Width(343));
				}

				if (Fog_EnabledRef != Fog_Enabled)
				{

					helper.Update_GlobalFog(mainCamera, Fog_Enabled, vFog, fDistance, fHeight, fheightDensity, fColor, fogIntensity, isMobile, fogIntensityHeight);
					UpdateSettings();

					if (LB_LightingProfile)
					{
						LB_LightingProfile.Fog_Enabled = Fog_Enabled;
						LB_LightingProfile.fogMode = vFog;
						EditorUtility.SetDirty(LB_LightingProfile);
					}
				}

				float fogIntensityRef = fogIntensity;
				float fogIntensityHeightRef = fogIntensityHeight;


				//-----Distance--------------------------------------------------------------------
				if (vFog == CustomFog.Distance)
				{
					float fDistanceRef = fDistance;
					Color fColorRef = fColor;

					if (fogState)
					{
						fogIntensity = (float)EditorGUILayout.Slider("Density", fogIntensity, 0, 30f);
						fColor = (Color)EditorGUILayout.ColorField("Color", fColor);
					}
					if (fDistanceRef != fDistance || fColorRef != fColor || fogIntensityRef != fogIntensity)
					{
						helper.Update_GlobalFog(mainCamera, Fog_Enabled, vFog, fDistance, fHeight, fheightDensity, fColor, fogIntensity, isMobile, fogIntensityHeight);
						UpdateSettings();
						if (LB_LightingProfile)
						{
							LB_LightingProfile.fogDistance = fDistance;
							LB_LightingProfile.fogColor = fColor;
							LB_LightingProfile.fogIntensity = fogIntensity;
							LB_LightingProfile.fogMode = vFog;
							EditorUtility.SetDirty(LB_LightingProfile);
						}
					}
				}
				//-----Height--------------------------------------------------------------------
				if (vFog == CustomFog.Height)
				{
					float fDistanceRef = fDistance;
					float fHeightRef = fHeight;
					Color fColorRef = fColor;
					float fheightDensityRef = fheightDensity;


					if (fogState)
					{
						fHeight = (float)EditorGUILayout.Slider("Height", fHeight, 0, 300f);
						fheightDensity = (float)EditorGUILayout.Slider("Height Density", fheightDensity, 0, 0.03f);
						fogIntensityHeight = 1f;
						fColor = (Color)EditorGUILayout.ColorField("Color", fColor);
					}
					if (fogIntensityHeightRef != fogIntensityHeight ||
						fogIntensityRef != fogIntensity ||
						fHeightRef != fHeight ||
						fheightDensityRef != fheightDensity ||
						fColorRef != fColor || fDistanceRef != fDistance)
					{
						helper.Update_GlobalFog(mainCamera, Fog_Enabled, vFog, 0, fHeight, fheightDensity, fColor, fogIntensity, isMobile, fogIntensityHeight);
						UpdateSettings();
						if (LB_LightingProfile)
						{
							LB_LightingProfile.fogHeight = fHeight;
							LB_LightingProfile.fogHeightIntensity = fheightDensity;
							LB_LightingProfile.fogColor = fColor;
							LB_LightingProfile.fogState = fogState;
							LB_LightingProfile.fogDistance = fDistance;
							LB_LightingProfile.fogMode = vFog;
							LB_LightingProfile.fogIntensityHeight = fogIntensityHeight;
							EditorUtility.SetDirty(LB_LightingProfile);
						}
					}
				}

				//-----Global--------------------------------------------------------------------
				if (vFog == CustomFog.Global)
				{
					Color fColorRef = fColor;

					if (fogState)
					{
						fogIntensity = (float)EditorGUILayout.Slider("Density", fogIntensity, 0, 40f);
						fColor = (Color)EditorGUILayout.ColorField("Color", fColor);
					}
					if (fColorRef != fColor || fogIntensityRef != fogIntensity)
					{
						helper.Update_GlobalFog(mainCamera, Fog_Enabled, vFog, 0, fHeight, fheightDensity, fColor, fogIntensity, isMobile, fogIntensityHeight);
						UpdateSettings();
						if (LB_LightingProfile)
						{
							LB_LightingProfile.fogColor = fColor;
							LB_LightingProfile.fogIntensity = fogIntensity;
							LB_LightingProfile.fogState = fogState;
							LB_LightingProfile.fogMode = vFog;
							LB_LightingProfile.fogIntensityHeight = fogIntensityHeight;
							EditorUtility.SetDirty(LB_LightingProfile);
						}
					}
				}

				//-----Global Fog Type--------------------------------------------------------------------
				if (vfogRef != vFog)
				{

					helper.Update_GlobalFog(mainCamera, Fog_Enabled, vFog, 0, fHeight, fheightDensity, fColor, fogIntensity, isMobile, fogIntensityHeight);
					UpdateSettings();

					//-------------------------------------------------------------------
					if (LB_LightingProfile)
					{
						LB_LightingProfile.fogMode = vFog;
						LB_LightingProfile.fogState = fogState;
						EditorUtility.SetDirty(LB_LightingProfile);
					}
				}

				#endregion

				if (targetPlatform == Target_Platform.FullFeatured || targetPlatform == Target_Platform.WebGL_2)
				{
					#region Depth of Field Legacy    

					//-----------Depth of Field----------------------------------------------------------------------------
					GUILayout.BeginVertical("Box");

					var DOF_EnabledRef = DOF_Enabled;

					EditorGUILayout.BeginHorizontal();

					if (dofState)
						GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
					else
						GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

					DOF_Enabled = EditorGUILayout.Toggle("", DOF_Enabled, GUILayout.Width(30f), GUILayout.Height(17f));

					var dofStateRef = dofState;

					if (GUILayout.Button("Depth of Field", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
					{
						dofState = !dofState;
					}

					if (dofStateRef != dofState)
					{
						if (LB_LightingProfile)
							LB_LightingProfile.dofState = dofState;
						if (LB_LightingProfile)
							EditorUtility.SetDirty(LB_LightingProfile);
					}

					EditorGUILayout.EndHorizontal();

					GUILayout.EndVertical();
					//---------------------------------------------------------------------------------------

					if (dofState)
					{
						if (helpBox)
							EditorGUILayout.HelpBox("Activate Depth Of Field for the camera", MessageType.Info);

					}
					var focusDistanceRef = focusDistance;
					var AperTureRef = AperTure;
					var fLengthRef = fLength;
					var kernelSizeRef = kernelSize;


					if (dofState)
					{
						focusDistance = (float)EditorGUILayout.Slider("Focus Distance", focusDistance, 0, 30f);
						AperTure = (float)EditorGUILayout.Slider("Aperture", AperTure, 0.1f, 32f);
						fLength = (float)EditorGUILayout.Slider("Focus Length", fLength, 1, 300f);
						kernelSize = (UnityEngine.Rendering.PostProcessing.KernelSize)EditorGUILayout.EnumPopup("Blur Size", kernelSize, GUILayout.Width(310));
					}


					if (DOF_EnabledRef != DOF_Enabled ||
						focusDistanceRef != focusDistance
					  || AperTureRef != AperTure ||
					  fLengthRef != fLength ||
						kernelSizeRef != kernelSize
					  )
					{

						helper.Update_DOF(DOF_Enabled, focusDistance, AperTure, fLength, kernelSize);

						//----------------------------------------------------------------------
						if (LB_LightingProfile)
						{
							LB_LightingProfile.DOF_Enabled = DOF_Enabled;
							LB_LightingProfile.focusDistance = focusDistance;
							LB_LightingProfile.AperTure = AperTure;
							LB_LightingProfile.kernelSize = kernelSize;
							LB_LightingProfile.fLength = fLength;
							EditorUtility.SetDirty(LB_LightingProfile);
						}
					}

					#endregion
				}
				else
				{
					DOF_Enabled = false;
					AutoFocus_Enabled = false;

					helper.Update_DOF(DOF_Enabled, focusDistance, AperTure, fLength, kernelSize);
				}

				#region Bloom

				//-----------Bloom----------------------------------------------------------------------------
				GUILayout.BeginVertical("Box");

				var Bloom_EnabledRef = Bloom_Enabled;

				EditorGUILayout.BeginHorizontal();

				if (bloomState)
					GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
				else
					GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

				Bloom_Enabled = EditorGUILayout.Toggle("", Bloom_Enabled, GUILayout.Width(30f), GUILayout.Height(17f));

				var bloomStateRef = bloomState;

				if (GUILayout.Button("Bloom", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
				{
					bloomState = !bloomState;
				}

				if (bloomStateRef != bloomState)
				{
					if (LB_LightingProfile)
						LB_LightingProfile.bloomState = bloomState;
					if (LB_LightingProfile)
						EditorUtility.SetDirty(LB_LightingProfile);
				}

				EditorGUILayout.EndHorizontal();

				GUILayout.EndVertical();
				//---------------------------------------------------------------------------------------
				if (bloomState)
				{
					if (helpBox)
						EditorGUILayout.HelpBox("Activate bloom for the camera", MessageType.Info);

				}
				var bIntensityRef = bIntensity;
				var bThreshouldRef = bThreshould;
				var bColorRef = bColor;
				var dirtTextureRef = dirtTexture;
				var dirtIntensityRef = dirtIntensity;
				var mobileOptimizedBloomRef = mobileOptimizedBloom;
				var bRotationRef = bRotation;
				if (bloomState)
				{
					bIntensity = (float)EditorGUILayout.Slider("Intensity", bIntensity, 0, 3f);
					bThreshould = (float)EditorGUILayout.Slider("Threshould", bThreshould, 0, 2f);
					bRotation = (float)EditorGUILayout.Slider("Rotation", bRotation, -1, 0.7f);

					bColor = (Color)EditorGUILayout.ColorField("Color", bColor);
					mobileOptimizedBloom = EditorGUILayout.Toggle("Fast Mode ( Mobile )", mobileOptimizedBloom);
					EditorGUILayout.Space();

					dirtTexture = EditorGUILayout.ObjectField("Dirt Texture", dirtTexture, typeof(Texture2D), true) as Texture2D;
					dirtIntensity = (float)EditorGUILayout.Slider("Dirt Intensity", dirtIntensity, 0, 10f);
				}

				if (dirtTextureRef != dirtTexture || dirtIntensityRef != dirtIntensity || Bloom_EnabledRef != Bloom_Enabled || bIntensityRef != bIntensity || bColorRef != bColor || bThreshouldRef != bThreshould || bIntensityRef != bIntensity
					|| mobileOptimizedBloomRef != mobileOptimizedBloom || bRotationRef != bRotation)
				{


					helper.Update_Bloom(Bloom_Enabled, bIntensity, bThreshould, bColor, dirtTexture, dirtIntensity, mobileOptimizedBloom, bRotation);


					//----------------------------------------------------------------------

					if (LB_LightingProfile)
					{
						LB_LightingProfile.Bloom_Enabled = Bloom_Enabled;
						LB_LightingProfile.bIntensity = bIntensity;
						LB_LightingProfile.bRotation = bRotation;
						LB_LightingProfile.bThreshould = bThreshould;
						LB_LightingProfile.mobileOptimizedBloom = mobileOptimizedBloom;
						LB_LightingProfile.bColor = bColor;
						LB_LightingProfile.dirtTexture = dirtTexture;
						LB_LightingProfile.dirtIntensity = dirtIntensity;
						EditorUtility.SetDirty(LB_LightingProfile);
					}
				}

				#endregion

			}

			if (winMode == WindowMode.Part3)
			{

				#region Color Grading

				//-----------Color Grading----------------------------------------------------------------------------
				GUILayout.BeginVertical("Box");

				EditorGUILayout.BeginHorizontal();

				if (colorState)
					GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
				else
					GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));


				var colorStateRef = colorState;

				if (GUILayout.Button("Color Grading", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
				{
					colorState = !colorState;
				}

				if (colorStateRef != colorState)
				{
					if (LB_LightingProfile)
						LB_LightingProfile.colorState = colorState;
					if (LB_LightingProfile)
						EditorUtility.SetDirty(LB_LightingProfile);
				}

				EditorGUILayout.EndHorizontal();

				GUILayout.EndVertical();
				//---------------------------------------------------------------------------------------

				if (colorState)
				{
					if (helpBox)
						EditorGUILayout.HelpBox("Color grading settings", MessageType.Info);

				}
				var colorModeRef = colorMode;
				var exposureIntensityRef = exposureIntensity;
				var contrastValueRef = contrastValue;
				var tempRef = temp;
				var gammaRef = gamma;
				var colorGammaRef = colorGamma;
				var colorLiftRef = colorLift;
				var saturationRef = saturation;
				var lutRef = lut;

				var exposureModeRef = exposureMode;
				var exposureCompensationRef = exposureCompensation;
				var eyeMinRef = eyeMin;
				var eyeMaxRef = eyeMax;

				if (colorState)
				{
					if (targetPlatform == Target_Platform.FullFeatured)
						colorMode = (ColorMode)EditorGUILayout.EnumPopup("Mode", colorMode, GUILayout.Width(343));

					if (colorMode == ColorMode.LUT)
					{
						lut = EditorGUILayout.ObjectField("LUT Texture   ", lut, typeof(Texture), true) as Texture;


					}
					else
					{


						if (targetPlatform == Target_Platform.FullFeatured)
						{
							EditorGUILayout.Space();
							EditorGUILayout.Space();
							EditorGUILayout.Space();

							exposureMode = (LB_ExposureMode)EditorGUILayout.EnumPopup("Exposure Mode", exposureMode, GUILayout.Width(343));

							EditorGUILayout.Space();


							exposureCompensation = (float)EditorGUILayout.Slider("Compensation", exposureCompensation, 0, 1f);

							if (exposureMode == LB_ExposureMode.Auto)
							{
								eyeMin = (float)EditorGUILayout.Slider("Min", eyeMin, -9, 9);
								eyeMax = (float)EditorGUILayout.Slider("Max", eyeMax, -9, 9);
							}



							EditorGUILayout.Space();
							EditorGUILayout.Space();

						}

						EditorGUILayout.Space();
						EditorGUILayout.Space();
						exposureIntensity = (float)EditorGUILayout.Slider("Overall Exposure", exposureIntensity, 0, 3f);
						contrastValue = (float)EditorGUILayout.Slider("Contrast", contrastValue, 0, 1f);
						gamma = (float)EditorGUILayout.Slider("Gamma", gamma, -1f, 1f);
						EditorGUILayout.Space();
						EditorGUILayout.Space();
						EditorGUILayout.Space();

						saturation = (float)EditorGUILayout.Slider("Saturation", saturation, -1f, 0.3f);
						temp = (float)EditorGUILayout.Slider("Temperature", temp, 0, 100f);

						EditorGUILayout.Space();
						EditorGUILayout.Space();
						EditorGUILayout.Space();

						colorGamma = (Color)EditorGUILayout.ColorField("Gamma Color", colorGamma);
						colorLift = (Color)EditorGUILayout.ColorField("Lift Color", colorLift);
						EditorGUILayout.Space();

					}
				}

				if (exposureIntensityRef != exposureIntensity || contrastValueRef != contrastValue || tempRef != temp || exposureCompensationRef != exposureCompensation
				  || colorModeRef != colorMode || gammaRef != gamma || colorGammaRef != colorGamma || colorLiftRef != colorLift || saturationRef != saturation
				|| lutRef != lut || exposureModeRef != exposureMode || eyeMinRef != eyeMin
				|| eyeMaxRef != eyeMax)
				{


					helper.Update_ColorGrading(colorMode, exposureIntensity, contrastValue, temp, exposureCompensation, saturation, colorGamma, colorLift, gamma, lut, exposureMode, eyeMin, eyeMax, targetPlatform);

					//----------------------------------------------------------------------
					if (LB_LightingProfile)
					{
						LB_LightingProfile.exposureIntensity = exposureIntensity;
						LB_LightingProfile.lut = lut;
						LB_LightingProfile.contrastValue = contrastValue;
						LB_LightingProfile.temp = temp;
						LB_LightingProfile.exposureCompensation = exposureCompensation;
						LB_LightingProfile.exposureMode = exposureMode;
						LB_LightingProfile.eyeMin = eyeMin;
						LB_LightingProfile.eyeMax = eyeMax;
						LB_LightingProfile.colorMode = colorMode;
						LB_LightingProfile.colorLift = colorLift;
						LB_LightingProfile.colorGamma = colorGamma;
						LB_LightingProfile.gamma = gamma;
						LB_LightingProfile.saturation = saturation;
						EditorUtility.SetDirty(LB_LightingProfile);
					}
				}

				#endregion

				#region Foliage
				/*

			//-----------Foliage----------------------------------------------------------------------------
			GUILayout.BeginVertical ("Box");


			EditorGUILayout.BeginHorizontal ();

			if(foliageState)
				GUILayout.Label(arrowOn,stateButton,GUILayout.Width(20),GUILayout.Height(14));
			else
				GUILayout.Label(arrowOff,stateButton,GUILayout.Width(20),GUILayout.Height(14));


			var foliageStateRef = foliageState;

			if (GUILayout.Button ("Foliage", stateButton, GUILayout.Width (300), GUILayout.Height (17f))) {
				foliageState = !foliageState;
			}

			if(foliageStateRef != foliageState)
			{
				if (LB_LightingProfile)
					LB_LightingProfile.foliageState = foliageState;
				if (LB_LightingProfile)
					EditorUtility.SetDirty (LB_LightingProfile);
			}

			EditorGUILayout.EndHorizontal ();

			GUILayout.EndVertical ();
			//---------------------------------------------------------------------------------------

			if(foliageState)
			{
				if (helpBox)
					EditorGUILayout.HelpBox ("Control lighting box foliage shader settings", MessageType.Info);
			}

			var translucencyRef = translucency;
			var ambientRef = ambient;
			var shadowsRef = shadows;
			var tranColorRef = tranColor;
			var windSpeedRef = windSpeed;
			var windScaleRef = windScale;

			if(foliageState)
			{
				GUILayout.Label ("Translucency", new GUILayoutOption[]{ GUILayout.Width (300), GUILayout.Height (20) });
				GUILayout.Box ("", new GUILayoutOption[]{ GUILayout.Width (300), GUILayout.Height (1) });

				tranColor = (Color)EditorGUILayout.ColorField ("Color", tranColor);
				translucency = (float)EditorGUILayout.Slider ("Intensity", translucency, 0, 1f);
				ambient = (float)EditorGUILayout.Slider ("Ambient", ambient, 0, 1f);
				shadows = (float)EditorGUILayout.Slider ("Shadow", shadows, 0, 1f);


				GUILayout.Label ("Wind", new GUILayoutOption[]{ GUILayout.Width (300), GUILayout.Height (20) });
				GUILayout.Box ("", new GUILayoutOption[]{ GUILayout.Width (300), GUILayout.Height (1) });

				EditorGUILayout.LabelField("Customize wind properties from each material");

				windSpeed = (float)EditorGUILayout.Slider ("Speed", windSpeed, 0, 10f);
				windScale = (float)EditorGUILayout.Slider ("Scale", windScale, 0, 100f);

				EditorGUILayout.Space ();
				EditorGUILayout.Space ();


			}
			if (translucencyRef != translucency || ambientRef != ambient
				|| shadowsRef != shadows || tranColorRef != tranColor || windSpeedRef != windSpeed || windScaleRef != windScale)
			{

				helper.Update_Foliage (translucency, ambient, shadows, windSpeed, windScale, tranColor);

				//----------------------------------------------------------------------
				if (LB_LightingProfile)
				{
					LB_LightingProfile.translucency = translucency;
					LB_LightingProfile.ambient = ambient;
					LB_LightingProfile.shadows = shadows;
					LB_LightingProfile.tranColor = tranColor;
					LB_LightingProfile.windSpeed = windSpeed;
					LB_LightingProfile.windScale = windScale;
					EditorUtility.SetDirty (LB_LightingProfile);
				}

			}
				*/
				#endregion

				#region Snow


				//-----------Snow----------------------------------------------------------------------------
				GUILayout.BeginVertical("Box");


				EditorGUILayout.BeginHorizontal();

				if (snowState)
					GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
				else
					GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));


				var snowStateRef = snowState;

				if (GUILayout.Button("Snow", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
				{
					snowState = !snowState;
				}

				if (snowStateRef != snowState)
				{
					if (LB_LightingProfile)
					{
						LB_LightingProfile.snowState = snowState;
						EditorUtility.SetDirty(LB_LightingProfile);
					}
				}

				EditorGUILayout.EndHorizontal();

				GUILayout.EndVertical();
				//---------------------------------------------------------------------------------------

				if (snowState)
				{
					if (helpBox)
						EditorGUILayout.HelpBox("Conrol lighting box snow shader settings", MessageType.Info);

				}

				var snowAlbedoRef = snowAlbedo;
				var snowNormalRef = snowNormal;
				var snowIntensityRef = snowIntensity;

				if (snowState)
				{
					snowIntensity = (float)EditorGUILayout.Slider("Intensity", snowIntensity, 0, 3f);
					EditorGUILayout.Space();
					snowAlbedo = EditorGUILayout.ObjectField("Snow Albedo", snowAlbedo, typeof(Texture2D), true) as Texture2D;
					snowNormal = EditorGUILayout.ObjectField("Snow Normal", snowNormal, typeof(Texture2D), true) as Texture2D;

					EditorGUILayout.Space();

				}
				if (snowAlbedoRef != snowAlbedo || snowNormalRef != snowNormal
					|| snowIntensityRef != snowIntensity)
				{

					helper.Update_Snow(snowAlbedo, snowNormal, snowIntensity);

					//----------------------------------------------------------------------
					if (LB_LightingProfile)
					{
						LB_LightingProfile.snowAlbedo = snowAlbedo;
						LB_LightingProfile.snowNormal = snowNormal;
						LB_LightingProfile.snowIntensity = snowIntensity;
						LB_LightingProfile.customShaderSnow = customShaderSnow;
						EditorUtility.SetDirty(LB_LightingProfile);
					}

				}
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.Space();


				#endregion

			}

			if (winMode == WindowMode.Finish)
			{

				#region Anti Aliasing

				//-----------Anti Aliasing----------------------------------------------------------------------------
				GUILayout.BeginVertical("Box");

				var AA_EnabledRef = AA_Enabled;

				EditorGUILayout.BeginHorizontal();

				if (aaState)
					GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
				else
					GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

				AA_Enabled = EditorGUILayout.Toggle("", AA_Enabled, GUILayout.Width(30f), GUILayout.Height(17f));

				var aaStateRef = aaState;

				if (GUILayout.Button("Anti Aliasing", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
				{
					aaState = !aaState;
				}

				if (aaStateRef != aaState)
				{
					if (LB_LightingProfile)
						LB_LightingProfile.aaState = aaState;
					if (LB_LightingProfile)
						EditorUtility.SetDirty(LB_LightingProfile);
				}

				EditorGUILayout.EndHorizontal();

				GUILayout.EndVertical();
				//---------------------------------------------------------------------------------------

				var aaModeRef = aaMode;

				if (aaState)
				{
					if (helpBox)
						EditorGUILayout.HelpBox("Activate anti aliasing for the camera , preferred to use TAA for next-gen platforms", MessageType.Info);

					aaMode = (AAMode)EditorGUILayout.EnumPopup("Anti Aliasing", aaMode, GUILayout.Width(343));
				}
				if (aaModeRef != aaMode || AA_EnabledRef != AA_Enabled)
				{

					helper.Update_AA(mainCamera, aaMode, AA_Enabled, targetPlatform);

					//----------------------------------------------------------------------
					if (LB_LightingProfile)
					{
						LB_LightingProfile.aaMode = aaMode;
						LB_LightingProfile.AA_Enabled = AA_Enabled;
					}

					if (LB_LightingProfile)
						EditorUtility.SetDirty(LB_LightingProfile);
				}

				#endregion

				if (targetPlatform == Target_Platform.FullFeatured || targetPlatform == Target_Platform.WebGL_2)
				{
					#region AO

					//-----------Ambient Occlusion----------------------------------------------------------------------------
					GUILayout.BeginVertical("Box");

					var AO_EnabledRef = AO_Enabled;

					EditorGUILayout.BeginHorizontal();

					if (aoState)
						GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
					else
						GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

					AO_Enabled = EditorGUILayout.Toggle("", AO_Enabled, GUILayout.Width(30f), GUILayout.Height(17f));

					var aoStateRef = aoState;

					if (GUILayout.Button("Ambient Occlusion", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
					{
						aoState = !aoState;
					}

					if (aoStateRef != aoState)
					{
						if (LB_LightingProfile)
							LB_LightingProfile.aoState = aoState;
						if (LB_LightingProfile)
							EditorUtility.SetDirty(LB_LightingProfile);
					}

					EditorGUILayout.EndHorizontal();

					GUILayout.EndVertical();
					//---------------------------------------------------------------------------------------

					if (aoState)
					{
						if (helpBox)
							EditorGUILayout.HelpBox("Activate AO for the camera", MessageType.Info);

					}
					var aoIntensityRef = aoIntensity;
					var ambientOnlyRef = ambientOnly;
					var aoTypeRef = aoType;
					var aoRadiusRef = aoRadius;
					var aoColorRef = aoColor;
					var aoQualityRef = aoQuality;




					if (aoState)
					{
						aoType = (AOType)EditorGUILayout.EnumPopup("Type", aoType, GUILayout.Width(343));
					}
					if (aoType == AOType.Modern)
					{
						if (aoState)
						{
							aoIntensity = (float)EditorGUILayout.Slider("Intensity", aoIntensity, 0, 2f);
							aoColor = (Color)EditorGUILayout.ColorField("Color", aoColor);
							ambientOnly = (bool)EditorGUILayout.Toggle("Ambient Only", ambientOnly);
						}
					}
					if (aoType == AOType.Classic)
					{
						if (aoState)
						{
							aoRadius = (float)EditorGUILayout.Slider("Radius", aoRadius, 0, 4.3f);
							aoIntensity = (float)EditorGUILayout.Slider("Intensity", aoIntensity, 0, 4f);
							aoColor = (Color)EditorGUILayout.ColorField("Color", aoColor);
							aoQuality = (AmbientOcclusionQuality)EditorGUILayout.EnumPopup("Quality", aoQuality, GUILayout.Width(343));
							ambientOnly = (bool)EditorGUILayout.Toggle("Ambient Only", ambientOnly);
						}
					}


					if (AO_EnabledRef != AO_Enabled || aoIntensityRef != aoIntensity || ambientOnlyRef != ambientOnly
					  || aoTypeRef != aoType || aoRadiusRef != aoRadius || aoColorRef != aoColor || aoQualityRef != aoQuality)
					{

						if (AO_Enabled)
							helper.Update_AO(mainCamera, true, aoType, aoRadius, aoIntensity, ambientOnly, aoColor, aoQuality);
						if (!AO_Enabled)
							helper.Update_AO(mainCamera, false, aoType, aoRadius, aoIntensity, ambientOnly, aoColor, aoQuality);

						//----------------------------------------------------------------------
						if (LB_LightingProfile)
						{
							LB_LightingProfile.AO_Enabled = AO_Enabled;
							LB_LightingProfile.aoIntensity = aoIntensity;
							LB_LightingProfile.ambientOnly = ambientOnly;
							LB_LightingProfile.aoColor = aoColor;
							LB_LightingProfile.aoRadius = aoRadius;
							LB_LightingProfile.aoType = aoType;
							LB_LightingProfile.aoQuality = aoQuality;
							EditorUtility.SetDirty(LB_LightingProfile);
						}
					}

					#endregion
				}
				else
				{
					AO_Enabled = false;
					helper.Update_AO(mainCamera, AO_Enabled, aoType, aoRadius, aoIntensity, ambientOnly, aoColor, aoQuality);
				}

				#region Vignette


				//-----------Vignette----------------------------------------------------------------------------
				GUILayout.BeginVertical("Box");

				var Vignette_EnabledRef = Vignette_Enabled;

				EditorGUILayout.BeginHorizontal();

				if (vignetteState)
					GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
				else
					GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

				Vignette_Enabled = EditorGUILayout.Toggle("", Vignette_Enabled, GUILayout.Width(30f), GUILayout.Height(17f));

				var vignetteStateRef = vignetteState;

				if (GUILayout.Button("Vignette", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
				{
					vignetteState = !vignetteState;
				}

				if (vignetteStateRef != vignetteState)
				{
					if (LB_LightingProfile)
						LB_LightingProfile.vignetteState = vignetteState;
					if (LB_LightingProfile)
						EditorUtility.SetDirty(LB_LightingProfile);
				}

				EditorGUILayout.EndHorizontal();

				GUILayout.EndVertical();
				//---------------------------------------------------------------------------------------

				if (vignetteState)
				{
					if (helpBox)
						EditorGUILayout.HelpBox("Activate vignette effect for your camera", MessageType.Info);
				}

				var vignetteIntensityRef = vignetteIntensity;

				if (vignetteState)
					vignetteIntensity = EditorGUILayout.Slider("Intensity", vignetteIntensity, 0, 0.3f);


				if (Vignette_EnabledRef != Vignette_Enabled || vignetteIntensityRef != vignetteIntensity)
				{
					helper.Update_Vignette(Vignette_Enabled, vignetteIntensity);
				}

				if (LB_LightingProfile)
				{
					LB_LightingProfile.Vignette_Enabled = Vignette_Enabled;
					LB_LightingProfile.vignetteIntensity = vignetteIntensity;
					EditorUtility.SetDirty(LB_LightingProfile);
				}

				#endregion

				#region Motion Blur


				//-----------Motion Blur----------------------------------------------------------------------------
				GUILayout.BeginVertical("Box");

				var MotionBlur_EnabledRef = MotionBlur_Enabled;

				EditorGUILayout.BeginHorizontal();

				if (motionBlurState)
					GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
				else
					GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

				MotionBlur_Enabled = EditorGUILayout.Toggle("", MotionBlur_Enabled, GUILayout.Width(30f), GUILayout.Height(17f));

				var motionBlurStateRef = motionBlurState;

				if (GUILayout.Button("Motion Blur", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
				{
					motionBlurState = !motionBlurState;
				}

				if (motionBlurStateRef != motionBlurState)
				{
					if (LB_LightingProfile)
						LB_LightingProfile.motionBlurState = motionBlurState;
					if (LB_LightingProfile)
						EditorUtility.SetDirty(LB_LightingProfile);
				}

				EditorGUILayout.EndHorizontal();

				GUILayout.EndVertical();
				//---------------------------------------------------------------------------------------

				if (motionBlurState)
				{
					if (helpBox)
						EditorGUILayout.HelpBox("Activate motion blur effect for your camera", MessageType.Info);
				}


				if (MotionBlur_EnabledRef != MotionBlur_Enabled)
				{
					helper.Update_MotionBlur(MotionBlur_Enabled);
				}

				if (LB_LightingProfile)
					LB_LightingProfile.MotionBlur_Enabled = MotionBlur_Enabled;
				if (LB_LightingProfile)
					EditorUtility.SetDirty(LB_LightingProfile);

				#endregion

				#region Chromattic Aberration


				//-----------Chromattic Aberration----------------------------------------------------------------------------
				GUILayout.BeginVertical("Box");

				var Chromattic_EnabledRef = Chromattic_Enabled;

				EditorGUILayout.BeginHorizontal();

				if (chromatticState)
					GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
				else
					GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

				Chromattic_Enabled = EditorGUILayout.Toggle("", Chromattic_Enabled, GUILayout.Width(30f), GUILayout.Height(17f));

				var chromatticStateRef = chromatticState;

				if (GUILayout.Button("Chromattic Aberration", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
				{
					chromatticState = !chromatticState;
				}

				if (chromatticStateRef != chromatticState)
				{
					if (LB_LightingProfile)
						LB_LightingProfile.chromatticState = chromatticState;
					if (LB_LightingProfile)
						EditorUtility.SetDirty(LB_LightingProfile);
				}

				EditorGUILayout.EndHorizontal();

				GUILayout.EndVertical();
				//---------------------------------------------------------------------------------------

				if (chromatticState)
				{
					if (helpBox)
						EditorGUILayout.HelpBox("Activate chromattic aberration effect for your camera", MessageType.Info);
				}

				var CA_IntensityRef = CA_Intensity;
				var mobileOptimizedChromatticRef = mobileOptimizedChromattic;

				if (chromatticState)
				{
					CA_Intensity = EditorGUILayout.Slider("Intensity", CA_Intensity, 0, 1f);
					mobileOptimizedChromattic = EditorGUILayout.Toggle("Mobile Optimized", mobileOptimizedChromattic);
				}

				if (Chromattic_EnabledRef != Chromattic_Enabled || CA_IntensityRef != CA_Intensity
				|| mobileOptimizedChromatticRef != mobileOptimizedChromattic)
				{
					helper.Update_ChromaticAberration(Chromattic_Enabled, CA_Intensity, mobileOptimizedChromattic);
				}

				if (LB_LightingProfile)
				{
					LB_LightingProfile.Chromattic_Enabled = Chromattic_Enabled;
					LB_LightingProfile.CA_Intensity = CA_Intensity;
					LB_LightingProfile.mobileOptimizedChromattic = mobileOptimizedChromattic;
					EditorUtility.SetDirty(LB_LightingProfile);
				}

				#endregion

				if (targetPlatform == Target_Platform.FullFeatured)
				{
					#region Screen Space Reflections


					//-----------Screen Space Reflections----------------------------------------------------------------------------
					GUILayout.BeginVertical("Box");

					var SSR_EnabledRef = SSR_Enabled;

					EditorGUILayout.BeginHorizontal();

					if (ssrState)
						GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
					else
						GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

					SSR_Enabled = EditorGUILayout.Toggle("", SSR_Enabled, GUILayout.Width(30f), GUILayout.Height(17f));

					var ssrStateRef = ssrState;

					if (GUILayout.Button("Screen Space Reflections", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
					{
						ssrState = !ssrState;
					}

					if (ssrStateRef != ssrState)
					{
						if (LB_LightingProfile)
							LB_LightingProfile.ssrState = ssrState;
						if (LB_LightingProfile)
							EditorUtility.SetDirty(LB_LightingProfile);
					}

					EditorGUILayout.EndHorizontal();

					GUILayout.EndVertical();
					//---------------------------------------------------------------------------------------

					if (ssrState)
					{
						if (helpBox)
							EditorGUILayout.HelpBox("Activate unity's screen space reflections effect for your camera", MessageType.Info);

					}

					var ssrQualityRef = ssrQuality;
					var ssrAttenRef = ssrAtten;
					var ssrFadeRef = ssrFade;

					if (ssrState)
					{
						ssrQuality = (ScreenSpaceReflectionPreset)EditorGUILayout.EnumPopup("Quality", ssrQuality, GUILayout.Width(343));
						ssrAtten = EditorGUILayout.Slider("Attention", ssrAtten, 0, 1);
						ssrFade = EditorGUILayout.Slider("Fade Distance", ssrFade, 0, 1);
					}

					if (SSR_EnabledRef != SSR_Enabled || ssrQualityRef != ssrQuality || ssrAttenRef != ssrAtten || ssrFadeRef != ssrFade)
					{
						helper.Update_SSR(mainCamera, SSR_Enabled, ssrQuality, ssrAtten, ssrFade);

						if (LB_LightingProfile)
						{
							LB_LightingProfile.SSR_Enabled = SSR_Enabled;
							LB_LightingProfile.ssrQuality = ssrQuality;
							LB_LightingProfile.ssrFade = ssrFade;
							LB_LightingProfile.ssrAtten = ssrAtten;
							EditorUtility.SetDirty(LB_LightingProfile);
						}
					}

					#endregion
				}
				else
				{
					SSR_Enabled = false;
					helper.Update_SSR(mainCamera, SSR_Enabled, ssrQuality, ssrAtten, ssrFade);
				}

				if (targetPlatform == Target_Platform.FullFeatured)
				{
					#region Stochastic Screen Space Reflections

					//-----------Stochastic Screen Space Reflections----------------------------------------------------------------------------
					GUILayout.BeginVertical("Box");

					var ST_SSR_EnabledRef = ST_SSR_Enabled;

					EditorGUILayout.BeginHorizontal();

					if (st_ssrState)
						GUILayout.Label(arrowOn, stateButton, GUILayout.Width(20), GUILayout.Height(14));
					else
						GUILayout.Label(arrowOff, stateButton, GUILayout.Width(20), GUILayout.Height(14));

					ST_SSR_Enabled = EditorGUILayout.Toggle("", ST_SSR_Enabled, GUILayout.Width(30f), GUILayout.Height(17f));

					var st_ssrStateRef = st_ssrState;

					if (GUILayout.Button("Stochastic SSR", stateButton, GUILayout.Width(300), GUILayout.Height(17f)))
					{
						st_ssrState = !st_ssrState;
					}

					if (st_ssrStateRef != st_ssrState)
					{
						if (LB_LightingProfile)
							LB_LightingProfile.st_ssrState = st_ssrState;
						if (LB_LightingProfile)
							EditorUtility.SetDirty(LB_LightingProfile);
					}

					EditorGUILayout.EndHorizontal();

					GUILayout.EndVertical();
					//---------------------------------------------------------------------------------------

					if (st_ssrState)
					{
						if (helpBox)
							EditorGUILayout.HelpBox("Activate stochastic screen space reflections effect for your camera", MessageType.Info);

					}

					var resolutionModeRef = resolutionMode;
					var debugPassRef = debugPass;
					var rayDistanceRef = rayDistance;
					var screenFadeSizeRef = screenFadeSize;
					var smoothnessRangeRef = smoothnessRange;

					if (st_ssrState)
					{
						resolutionMode = (ResolutionMode)EditorGUILayout.EnumPopup("Resolution", resolutionMode, GUILayout.Width(343));
						rayDistance = EditorGUILayout.IntSlider("Ray Distance", rayDistance, 0, 100);
						screenFadeSize = EditorGUILayout.Slider("Fade Distance", screenFadeSize, 0, 1);
						smoothnessRange = EditorGUILayout.Slider("Smoothness Range", smoothnessRange, 0, 1);
						debugPass = (SSRDebugPass)EditorGUILayout.EnumPopup("DebugMode", debugPass, GUILayout.Width(343));
					}

					if (ST_SSR_EnabledRef != ST_SSR_Enabled || resolutionModeRef != resolutionMode
						|| debugPassRef != debugPass || screenFadeSizeRef != screenFadeSize
						|| rayDistanceRef != rayDistance || smoothnessRangeRef != smoothnessRange)
					{
						helper.Update_StochasticSSR(mainCamera, ST_SSR_Enabled, resolutionMode, debugPass, rayDistance, screenFadeSize, smoothnessRange);

						if (LB_LightingProfile)
						{
							LB_LightingProfile.ST_SSR_Enabled = ST_SSR_Enabled;
							LB_LightingProfile.resolutionMode = resolutionMode;
							LB_LightingProfile.screenFadeSize = screenFadeSize;
							LB_LightingProfile.smoothnessRange = smoothnessRange;
							LB_LightingProfile.debugPass = debugPass;
							LB_LightingProfile.rayDistance = rayDistance;
							EditorUtility.SetDirty(LB_LightingProfile);
						}
					}

					#endregion
				}
				else
				{
					ST_SSR_Enabled = false;
					helper.Update_StochasticSSR(mainCamera, ST_SSR_Enabled, resolutionMode, debugPass, rayDistance, screenFadeSize, smoothnessRange);
				}

				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.HelpBox("Good News , Version 3 Released !", MessageType.Info);
				if (GUILayout.Button("Buy Lighting Box 3 (50% OFF)"))
					Application.OpenURL("https://u3d.as/3t9W");
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.Space(); EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.Space();
			}
			EditorGUILayout.EndScrollView();
		}


		#region Update Settings
		void UpdateSettings()
		{

			// Sun Light Update
			if (sunLight)
			{
				sunLight.color = sunColor;
				sunLight.intensity = sunIntensity;
				sunLight.bounceIntensity = indirectIntensity;
			}
			else
			{
				Update_Sun();
			}

			if (sunFlare)
			{
				if (sunLight)
					sunLight.flare = sunFlare;
			}
			else
			{
				if (sunLight)
					sunLight.flare = null;
			}

			// Skybox
			helper.Update_SkyBox(Ambient_Enabled, skyBox, skyIntensity, skyCube, skyBoxExposure, skyRotation);

			// Update Lighting Mode
			helper.Update_LightingMode(Scene_Enabled, lightingMode);

			// Update Ambient
			helper.Update_Ambient(Ambient_Enabled, ambientLight, ambientColor, skyColor, equatorColor, groundColor);

			// Lights settings
			helper.Update_LightSettings(Scene_Enabled, lightSettings);

			// Color Space
			helper.Update_ColorSpace(Scene_Enabled, colorSpace);

			// Render Path
			helper.Update_RenderPath(Scene_Enabled, renderPath, mainCamera);

			// Volumetric Light
			helper.Update_VolumetricLight(mainCamera, VL_Enabled, vLight, vLightLevel);

			// Sun Shaft
			if (sunLight)
				helper.Update_SunShaft(mainCamera, SunShaft_Enabled, shaftQuality, shaftDistance, shaftBlur, shaftColor, sunLight.transform);

			// Shadows
			helper.Update_Shadows(Scene_Enabled, psShadow);

			// Light Probes
			helper.Update_LightProbes(Scene_Enabled, lightprobeMode);

			// Auto Mode
			helper.Update_AutoMode(autoMode);

			// Global Fog
			helper.Update_GlobalFog(mainCamera, Fog_Enabled, vFog, fDistance, fHeight, fheightDensity, fColor, fogIntensity, isMobile, fogIntensityHeight);

		}
		#endregion

		#region On Load
		// load saved data based on project and scene name
		void OnLoad()
		{

			if (!mainCamera)
			{
				if (GameObject.Find(LB_LightingProfile.mainCameraName))
					mainCamera = GameObject.Find(LB_LightingProfile.mainCameraName).GetComponent<Camera>();
				else
					mainCamera = GameObject.FindObjectOfType<Camera>();
			}

			if (!GameObject.Find("LightingBox_Helper"))
			{
				GameObject helperObject = new GameObject("LightingBox_Helper");
				helperObject.AddComponent<LB_LightingBoxHelper>();
				helper = helperObject.GetComponent<LB_LightingBoxHelper>();
			}


			if (LB_LightingProfile)
			{

				lightingMode = LB_LightingProfile.lightingMode;
				if (LB_LightingProfile.skyBox)
					skyBox = LB_LightingProfile.skyBox;
				else
					skyBox = RenderSettings.skybox;
				sunFlare = LB_LightingProfile.sunFlare;
				ambientLight = LB_LightingProfile.ambientLight;
				renderPath = LB_LightingProfile.renderPath;
				lightSettings = LB_LightingProfile.lightSettings;
				sunColor = LB_LightingProfile.sunColor;

				// Color Space
				colorSpace = LB_LightingProfile.colorSpace;

				// Volumetric Light
				vLight = LB_LightingProfile.vLight;
				vLightLevel = LB_LightingProfile.vLightLevel;

				lightprobeMode = LB_LightingProfile.lightProbesMode;

				// Shadows
				psShadow = LB_LightingProfile.lightsShadow;

				// Fog
				vFog = LB_LightingProfile.fogMode;
				fDistance = LB_LightingProfile.fogDistance;
				fHeight = LB_LightingProfile.fogHeight;
				fheightDensity = LB_LightingProfile.fogHeightIntensity;
				fColor = LB_LightingProfile.fogColor;
				fogIntensity = LB_LightingProfile.fogIntensity;
				fogIntensityHeight = LB_LightingProfile.fogIntensityHeight;

				// DOF 3
				focusDistance = LB_LightingProfile.focusDistance;
				AperTure = LB_LightingProfile.AperTure;
				fLength = LB_LightingProfile.fLength;
				kernelSize = LB_LightingProfile.kernelSize;

				// AA
				aaMode = LB_LightingProfile.aaMode;

				// AO
				aoIntensity = LB_LightingProfile.aoIntensity;
				aoColor = LB_LightingProfile.aoColor;
				ambientOnly = LB_LightingProfile.ambientOnly;
				aoRadius = LB_LightingProfile.aoRadius;
				aoType = LB_LightingProfile.aoType;
				aoQuality = LB_LightingProfile.aoQuality;

				// Bloom
				bIntensity = LB_LightingProfile.bIntensity;
				bColor = LB_LightingProfile.bColor;
				bThreshould = LB_LightingProfile.bThreshould;
				dirtTexture = LB_LightingProfile.dirtTexture;
				dirtIntensity = LB_LightingProfile.dirtIntensity;
				mobileOptimizedBloom = LB_LightingProfile.mobileOptimizedBloom;
				bRotation = LB_LightingProfile.bRotation;

				// Color Grading
				exposureIntensity = LB_LightingProfile.exposureIntensity;
				contrastValue = LB_LightingProfile.contrastValue;
				temp = LB_LightingProfile.temp;
				exposureCompensation = LB_LightingProfile.exposureCompensation;

				exposureMode = LB_LightingProfile.exposureMode;
				eyeMin = LB_LightingProfile.eyeMin;
				eyeMax = LB_LightingProfile.eyeMax;

				colorMode = LB_LightingProfile.colorMode;
				colorGamma = LB_LightingProfile.colorGamma;
				colorLift = LB_LightingProfile.colorLift;
				gamma = LB_LightingProfile.gamma;
				saturation = LB_LightingProfile.saturation;
				lut = LB_LightingProfile.lut;

				// Effects
				MotionBlur_Enabled = LB_LightingProfile.MotionBlur_Enabled;
				Vignette_Enabled = LB_LightingProfile.Vignette_Enabled;
				vignetteIntensity = LB_LightingProfile.vignetteIntensity;
				Chromattic_Enabled = LB_LightingProfile.Chromattic_Enabled;
				CA_Intensity = LB_LightingProfile.CA_Intensity;
				mobileOptimizedChromattic = LB_LightingProfile.mobileOptimizedChromattic;

				// SSR
				SSR_Enabled = LB_LightingProfile.SSR_Enabled;
				ssrQuality = LB_LightingProfile.ssrQuality;
				ssrAtten = LB_LightingProfile.ssrAtten;
				ssrFade = LB_LightingProfile.ssrFade;
				SSR_Enabled = LB_LightingProfile.SSR_Enabled;

				// Stochastic SSR
				resolutionMode = LB_LightingProfile.resolutionMode;
				debugPass = LB_LightingProfile.debugPass;
				rayDistance = LB_LightingProfile.rayDistance;
				screenFadeSize = LB_LightingProfile.screenFadeSize;
				smoothnessRange = LB_LightingProfile.smoothnessRange;
				ST_SSR_Enabled = LB_LightingProfile.ST_SSR_Enabled;


				// Lightmap
				bakedResolution = LB_LightingProfile.bakedResolution;
				sunIntensity = LB_LightingProfile.sunIntensity;
				indirectIntensity = LB_LightingProfile.indirectIntensity;
				enlightenQuality = LB_LightingProfile.enlightenQuality;

				ambientColor = LB_LightingProfile.ambientColor;
				ambientLight = LB_LightingProfile.ambientLight;
				skyIntensity = LB_LightingProfile.skyIntensity;
				skyBox = LB_LightingProfile.skyBox;
				skyRotation = LB_LightingProfile.skyRotation;
				skyBoxExposure = LB_LightingProfile.skyBoxExposure;
				skyCube = LB_LightingProfile.skyCube;
				skyColor = LB_LightingProfile.skyColor;
				equatorColor = LB_LightingProfile.equatorColor;
				groundColor = LB_LightingProfile.groundColor;

				// Auto lightmap
				autoMode = LB_LightingProfile.automaticLightmap;

				// Target platform (Full featured, WebGL 2, Mobile)
				targetPlatform = LB_LightingProfile.targetPlatform;

				if (targetPlatform == Target_Platform.Mobile)
					isMobile = true;
				else
					isMobile = false;

				// Sun Shaft
				shaftDistance = LB_LightingProfile.shaftDistance;
				shaftBlur = LB_LightingProfile.shaftBlur;
				shaftColor = LB_LightingProfile.shaftColor;
				shaftQuality = LB_LightingProfile.shaftQuality;

				/*
				// Foliage
				matType = LB_LightingProfile.matType;
				translucency = LB_LightingProfile.translucency;
				ambient = LB_LightingProfile.ambient;
				shadows = LB_LightingProfile.shadows;
				tranColor = LB_LightingProfile.tranColor;
				windSpeed = LB_LightingProfile.windSpeed;
				windScale = LB_LightingProfile.windScale;
				CustomShader = LB_LightingProfile.CustomShader;
				*/
				// Snow
				snowAlbedo = LB_LightingProfile.snowAlbedo;
				snowNormal = LB_LightingProfile.snowNormal;
				snowIntensity = LB_LightingProfile.snowIntensity;
				customShaderSnow = LB_LightingProfile.customShaderSnow;

				Ambient_Enabled = LB_LightingProfile.Ambient_Enabled;
				Scene_Enabled = LB_LightingProfile.Scene_Enabled;
				Sun_Enabled = LB_LightingProfile.Sun_Enabled;
				VL_Enabled = LB_LightingProfile.VL_Enabled;
				SunShaft_Enabled = LB_LightingProfile.SunShaft_Enabled;
				Fog_Enabled = LB_LightingProfile.Fog_Enabled;
				AutoFocus_Enabled = LB_LightingProfile.AutoFocus_Enabled;
				DOF_Enabled = LB_LightingProfile.DOF_Enabled;
				Bloom_Enabled = LB_LightingProfile.Bloom_Enabled;
				AA_Enabled = LB_LightingProfile.AA_Enabled;
				AO_Enabled = LB_LightingProfile.AO_Enabled;

				buildState = LB_LightingProfile.buildState;
				profileState = LB_LightingProfile.profileState;
				cameraState = LB_LightingProfile.cameraState;
				lightSettingsState = LB_LightingProfile.lightSettingsState;
				sunState = LB_LightingProfile.sunState;
				ambientState = LB_LightingProfile.ambientState;
				ssrState = LB_LightingProfile.ssrState;
				st_ssrState = LB_LightingProfile.st_ssrState;


				chromatticState = LB_LightingProfile.chromatticState;
				vignetteState = LB_LightingProfile.vignetteState;
				motionBlurState = LB_LightingProfile.motionBlurState;
				aoState = LB_LightingProfile.aoState;
				aaState = LB_LightingProfile.aaState;
				bloomState = LB_LightingProfile.bloomState;
				colorState = LB_LightingProfile.colorState;
				autoFocusState = LB_LightingProfile.autoFocusState;
				dofState = LB_LightingProfile.dofState;
				fogState = LB_LightingProfile.fogState;
				sunShaftState = LB_LightingProfile.sunShaftState;
				vLightState = LB_LightingProfile.vLightState;
				foliageState = LB_LightingProfile.foliageState;
				snowState = LB_LightingProfile.snowState;
				OptionsState = LB_LightingProfile.OptionsState;
				LightingBoxState = LB_LightingProfile.LightingBoxState;

				mainCamera.allowHDR = true;
				mainCamera.allowMSAA = false;


				if (LB_LightingProfile.postProcessingProfile)
					postProcessingProfile = LB_LightingProfile.postProcessingProfile;
			}

			UpdatePostEffects();

			UpdateSettings();

			Update_Sun();

		}
		#endregion

		#region Update Post Effects Settings

		public void UpdatePostEffects()
		{

			if (!helper)
				helper = GameObject.Find("LightingBox_Helper").GetComponent<LB_LightingBoxHelper>();

			if (!postProcessingProfile)
				return;

			helper.UpdateProfiles(mainCamera, postProcessingProfile);

			// MotionBlur
			if (MotionBlur_Enabled)
				helper.Update_MotionBlur(true);
			else
				helper.Update_MotionBlur(false);

			// Vignette
			helper.Update_Vignette(Vignette_Enabled, vignetteIntensity);


			// _ChromaticAberration
			helper.Update_ChromaticAberration(Chromattic_Enabled, CA_Intensity, mobileOptimizedChromattic);

			// Foliage
			//helper.Update_Foliage (translucency, ambient, shadows, windSpeed, windScale, tranColor);

			// Snow
			helper.Update_Snow(snowAlbedo, snowNormal, snowIntensity);

			helper.Update_Bloom(Bloom_Enabled, bIntensity, bThreshould, bColor, dirtTexture, dirtIntensity, mobileOptimizedBloom, bRotation);



			// Depth of Field 3
			helper.Update_DOF(DOF_Enabled, focusDistance, AperTure, fLength, kernelSize);

			// AO
			if (AO_Enabled)
				helper.Update_AO(mainCamera, true, aoType, aoRadius, aoIntensity, ambientOnly, aoColor, aoQuality);
			else
				helper.Update_AO(mainCamera, false, aoType, aoRadius, aoIntensity, ambientOnly, aoColor, aoQuality);


			// Color Grading
			helper.Update_ColorGrading(colorMode, exposureIntensity, contrastValue, temp, exposureCompensation, saturation, colorGamma, colorLift, gamma, lut, exposureMode, eyeMin, eyeMax, targetPlatform);

			////-----------------------------------------------------------------------------
			/// 
			// Screen Space Reflections
			helper.Update_SSR(mainCamera, SSR_Enabled, ssrQuality, ssrAtten, ssrFade);

			helper.Update_StochasticSSR(mainCamera, ST_SSR_Enabled, resolutionMode, debugPass, rayDistance, screenFadeSize, smoothnessRange);

		}
		#endregion

		#region Scene Delegate

		string currentScene;
		void SceneChanging()
		{
			if (currentScene != EditorSceneManager.GetActiveScene().name)
			{
				if (System.String.IsNullOrEmpty(ConfigUtility.GetConfig(EditorSceneManager.GetActiveScene().name)))
					LB_LightingProfile = Resources.Load("DefaultSettings") as LB_LightingProfile;
				else
					LB_LightingProfile = (LB_LightingProfile)AssetDatabase.LoadAssetAtPath(ConfigUtility.GetConfig(EditorSceneManager.GetActiveScene().name), typeof(LB_LightingProfile));

				helper.Update_MainProfile(LB_LightingProfile);

				OnLoad();
				currentScene = EditorSceneManager.GetActiveScene().name;
			}

		}
		#endregion

		#region Sun Light
		public void Update_Sun()
		{
			if (Sun_Enabled)
			{
				if (!RenderSettings.sun)
				{
					Light[] lights = GameObject.FindObjectsOfType<Light>();
					foreach (Light l in lights)
					{
						if (l.type == LightType.Directional)
						{
							sunLight = l;

							if (sunColor != Color.clear)
								sunColor = l.color;
							else
								sunColor = Color.white;

							//sunLight.shadowNormalBias = 0.05f;  
							sunLight.color = sunColor;
							if (sunLight.bounceIntensity == 1f)
								sunLight.bounceIntensity = indirectIntensity;
						}
					}
				}
				else
				{
					sunLight = RenderSettings.sun;

					if (sunColor != Color.clear)
						sunColor = sunLight.color;
					else
						sunColor = Color.white;

					//	sunLight.shadowNormalBias = 0.05f;  
					sunLight.color = sunColor;
					if (sunLight.bounceIntensity == 1f)
						sunLight.bounceIntensity = indirectIntensity;
				}
			}
		}

		#endregion

		#region On Download Completed
		void DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
		{
			if (e.Error != null)
				Debug.Log(e.Error);
		}
		#endregion
	}
}
#endif