
/*  This library reads version 4.3 and compatible ICC-profiles as specified by the International Color Consortium.
    Copyright (C) 2013  Johannes Bildstein

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.*/

namespace ICCReader
{
    public enum ProfileClassName : uint
    {
        InputDevice = 0x73636E72,       //scnr
        DisplayDevice = 0x6D6E7472,     //mntr
        OutputDevice = 0x70727472,      //prtr
        DeviceLink = 0x6C696E6B,        //link
        ColorSpace = 0x73706163,        //spac
        Abstract = 0x61627374,          //abst
        NamedColor = 0x6E6D636C,        //nmcl
    }

    public enum ColorSpaceType : uint
    {
        CIEXYZ = 0x58595A20,        //XYZ
        CIELAB = 0x4C616220,        //Lab
        CIELUV = 0x4C757620,        //Luv
        YCbCr = 0x59436272,         //YCbr
        CIEYxy = 0x59787920,        //Yxy
        RGB = 0x52474220,           //RGB
        Gray = 0x47524159,          //GRAY
        HSV = 0x48535620,           //HSV
        HLS = 0x484C5320,           //HLS
        CMYK = 0x434D594B,          //CMYK
        CMY = 0x434D5920,           //CMY
        Color2 = 0x32434C52,       //2CLR
        Color3 = 0x33434C52,       //3CLR
        Color4 = 0x34434C52,       //4CLR
        Color5 = 0x35434C52,       //5CLR
        Color6 = 0x36434C52,       //6CLR
        Color7 = 0x37434C52,       //7CLR
        Color8 = 0x38434C52,       //8CLR
        Color9 = 0x39434C52,       //9CLR
        Color10 = 0x41434C52,      //ACLR
        Color11 = 0x42434C52,      //BCLR
        Color12 = 0x43434C52,      //CCLR
        Color13 = 0x44434C52,      //DCLR
        Color14 = 0x45434C52,      //ECLR
        Color15 = 0x46434C52,      //FCLR
    }

    public enum PrimaryPlatformName : uint
    {
        NotIdentified = 0x00000000,
        AppleComputerInc = 0x4150504C,          //APPL
        MicrosoftCorporation = 0x4D534654,      //MSFT
        SiliconGraphicsInc = 0x53474920,        //SGI
        SunMicrosystemsInc = 0x53554E57,        //SUNW
    }

    public enum DeviceAttributeName
    {
        Reflective,
        Transparency,
        Glossy,
        Matte,
        Positive,
        Negative,
        Color,
        BlackWhite,
    }

    public enum RenderingIntentName : uint
    {
        Perceptual = 0,
        MediaRelativeColorimetric = 1,
        Saturation = 2,
        ICCAbsoluteColorimetric = 3,
    }

    public enum TagSignature : uint
    {
        AToB0Tag = 0x41324230, 					            //A2B0
        AToB1Tag = 0x41324231,					            //A2B1
        AToB2Tag = 0x41324232,					            //A2B2
        blueMatrixColumnTag = 0x6258595A, 					//bXYZ
        blueTRCTag = 0x62545243,					        //bTRC
        BToA0Tag = 0x42324130,					            //B2A0
        BToA1Tag = 0x42324131,					            //B2A1
        BToA2Tag = 0x42324132,					            //B2A2
        BToD0Tag = 0x42324430,					            //B2D0
        BToD1Tag = 0x42324431,					            //B2D1
        BToD2Tag = 0x42324432,					            //B2D2
        BToD3Tag = 0x42324433,					            //B2D3
        calibrationDateTimeTag = 0x63616C74,				//calt
        charTargetTag = 0x74617267,					        //targ
        chromaticAdaptationTag = 0x63686164,				//chad
        chromaticityTag = 0x6368726D,					    //chrm
        colorantOrderTag = 0x636C726F,					    //clro
        colorantTableTag = 0x636C7274,					    //clrt
        colorantTableOutTag = 0x636C6F74,					//clot
        colorimetricIntentImageStateTag = 0x63696973,		//ciis
        copyrightTag = 0x63707274,					        //cprt
        deviceMfgDescTag = 0x646D6E64,					    //dmnd
        deviceModelDescTag = 0x646D6464,					//dmdd
        DToB0Tag = 0x44324230,					            //D2B0
        DToB1Tag = 0x44324231,					            //D2B1
        DToB2Tag = 0x44324232,					            //D2B2
        DToB3Tag = 0x44324233,					            //D2B3
        gamutTag = 0x67616D74,					            //gamt
        grayTRCTag = 0x6B545243,					        //kTRC
        greenMatrixColumnTag = 0x6758595A,					//gXYZ
        greenTRCTag = 0x67545243,					        //gTRC
        luminanceTag = 0x6C756D69,					        //lumi
        measurementTag = 0x6D656173,					    //meas
        mediaWhitePointTag = 0x77747074,					//wtpt
        namedColor2Tag = 0x6E636C32,					    //ncl2
        outputResponseTag = 0x72657370,					    //resp
        perceptualRenderingIntentGamutTag = 0x72696730,		//rig0
        preview0Tag = 0x70726530,					        //pre0
        preview1Tag = 0x70726531,					        //pre1
        preview2Tag = 0x70726532,					        //pre2
        profileDescriptionTag = 0x64657363,					//desc
        profileSequenceDescTag = 0x70736571,				//pseq
        profileSequenceIdentifierTag = 0x70736964,			//psid
        redMatrixColumnTag = 0x7258595A,					//rXYZ
        redTRCTag = 0x72545243,					            //rTRC
        saturationRenderingIntentGamutTag = 0x72696732,		//rig2
        technologyTag = 0x74656368,					        //tech
        viewingCondDescTag = 0x76756564,					//vued
        viewingConditionsTag = 0x76696577,					//view
    }
    
    public enum TypeSignature : uint
    {
        Unknown,

        chromaticity = 0x6368726D,
        colorantOrder = 0x636c726f,
        colorantTable = 0x636c7274,
        curve = 0x63757276,
        data = 0x64617461,
        dateTime = 0x6474696D,
        lut16 = 0x6D667432,
        lut8 = 0x6D667431,
        lutAToB = 0x6D414220,
        lutBToA = 0x6D424120,
        measurement = 0x6D656173,
        multiLocalizedUnicode = 0x6D6C7563,
        multiProcessElements = 0x6D706574,
        namedColor2 = 0x6E636C32,
        parametricCurve = 0x70617261,
        profileSequenceDesc = 0x70736571,
        profileSequenceIdentifier = 0x70736964,
        responseCurveSet16 = 0x72637332,
        s15Fixed16Array = 0x73663332,
        signature = 0x73696720,
        text = 0x74657874,
        u16Fixed16Array = 0x75663332,
        uInt16Array = 0x75693136,
        uInt32Array = 0x75693332,
        uInt64Array = 0x75693634,
        uInt8Array = 0x75693038,
        viewingConditions = 0x76696577,
        XYZ = 0x58595A20,
    }

    public enum SignatureName : uint
    {
        Unknown = 0,

        //colorimetric intent image state
        SceneColorimetryEstimates = 0x73636F65,//scoe
        SceneAppearanceEstimates = 0x73617065,//sape
        FocalPlaneColorimetryEstimates = 0x66706365,//fpce
        ReflectionHardcopyOriginalColorimetry = 0x72686F63,//rhoc
        ReflectionPrintOutputColorimetry = 0x72706F63,//rpoc

        //Rendering intent gamut
        PerceptualReferenceMediumGamut = 0x70726D67,//prmg

        //Technology
        FilmScanner = 0x6673636E,//fscn
        DigitalCamera = 0x6463616D,//dcam
        ReflectiveScanner = 0x7273636E,//rscn
        InkJetPrinter = 0x696A6574,//ijet
        ThermalWaxPrinter = 0x74776178,//twax
        ElectrophotographicPrinter = 0x6570686F,//epho
        ElectrostaticPrinter = 0x65737461,//esta
        DyeSublimationPrinter = 0x64737562,//dsub
        PhotographicPaperPrinter = 0x7270686F,//rpho
        FilmWriter = 0x6670726E,//fprn
        VideoMonitor = 0x7669646D,//vidm
        VideoCamera = 0x76696463,//vidc
        ProjectionTelevision = 0x706A7476,//pjtv
        CathodeRayTubeDisplay = 0x43525420,//CRT
        PassiveMatrixDisplay = 0x504D4420,//PMD 
        ActiveMatrixDisplay = 0x414D4420,//AMD 
        PhotoCD = 0x4B504344,//KPCD
        PhotographicImageSetter = 0x696D6773,//imgs
        Gravure = 0x67726176,//grav
        OffsetLithography = 0x6F666673,//offs
        Silkscreen = 0x73696C6B,//silk
        Flexography = 0x666C6578,//flex
        MotionPictureFilmScanner = 0x6D706673,//mpfs
        MotionPictureFilmRecorder = 0x6D706672,//mpfr
        DigitalMotionPictureCamera = 0x646D7063,//dmpc
        DigitalCinemaProjector = 0x64636A70,//dcpj
    }

    public enum multiProcessElementSignature : uint
    {
        Unknown = 0,
        CurveSet = 0x6D666C74,  //cvst
        Matrix = 0x6D617466,    //matf
        CLUT = 0x636C7574,      //clut
        bACS = 0x62414353,      //bACS
        eACS = 0x65414353,      //eACS
    }
    
    public enum CurveSegmentSignature : uint
    {
        FormulaCurve = 0x70617266,          //parf
        SampledCurve = 0x73616D66,          //samf
    }

    public enum ColorantEncoding : uint
    {
        Unknown = 0x0000,
        ITU_R_BT_709_2 = 0x0001,
        SMPTE_RP145 = 0x0002,
        EBU_Tech_3213_E = 0x0003,
        P22 = 0x0004,
    }

    public enum StandardObserver : uint
    {
        Unkown = 0,
        CIE1931StandardColorimetricObserver = 1,
        CIE1964StandardColorimetricObserver = 2,
    }

    public enum MeasurementGeometry : uint
    {
        Unknown = 0,
        MG_0_45or45_0 = 1,
        MG_0d_or_d0 = 2,
    }

    public enum StandardIlluminant : uint
    {
        Unknown = 0,
        D50 = 1,
        D65 = 2,
        D93 = 3,
        F2 = 4,
        D55 = 5,
        A = 6,
        Equi_Power_E = 7,
        F8 = 8,
    }
    
    public enum CurveMeasurementEncodings : uint
    {
        StatusA = 0x53746141,   //StaA
        StatusE = 0x53746145,   //StaE
        StatusI = 0x53746149,   //StaI
        StatusT = 0x53746154,   //StaT
        StatusM = 0x5374614D,   //StaM
        DinE = 0x434E2020,      //DN
        DinE_pol = 0x434E2050,  //DNP
        DinI = 0x434E4E20,      //DNN
        DinI_pol = 0x434E4E50,  //DNNP
    }
}