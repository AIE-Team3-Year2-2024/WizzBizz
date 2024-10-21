void IfAnyNonZeroReturnOneElseZero_float(float A, float B, float C, float D, out float output)
{
	output = 0.0f;
	if (A != 0.0f || B != 0.0f || C != 0.0f || D != 0.0f)
		output = 1.0f;
}