import { ref, onMounted  } from "vue";
import type { ColorScheme } from "@/model/ColorScheme";

export default function useColorScheme() {
	//Dev
	const colorSchemeDev = ref<ColorScheme>({
		background: "#F5F5F5",
		backgroundSecond: "#dcf9fc",
		backgroundThird: "#dcfce7",
		backgroundNavAndFooter: "#576f72",
		primary: "#8E8D8A",
		secondary: "#FFFFFF",
		tertiary: "#FFFFFF",
		text: "#000000",
        textDark: "#8E8D8A",
        textLight: "#000000",
		textNavLink: "#FFFFFF"
	});

	// const colorScheme = ref<ColorScheme>({
	// 	background: "#F5F5F5", // Light grey
	// 	primary: "#0000FF", // Blue
	// 	secondary: "#FFFFFF", // White
	// 	tertiary: "",
	// 	text: "#000000", // Black
	// });

	//Pale beige and red
	const colorSchemePaleBeige = ref<ColorScheme>({
		background: "#EAE7DC", // beige
		backgroundSecond: "#D8C3A5", //darker beige
		backgroundThird: "#dcfce7",
		backgroundNavAndFooter: "#EAE7DC", //beige
		primary: "#E85A4F", //red
		secondary: "#E98074", // brighter red
		tertiary: "#FFFFFF",
		text: "#000000",
		textDark: "#8E8D8A",
        textLight: "#000000",
		textNavLink: "#FFFFFF"
	});

	onMounted(() => {
		// Insert the color scheme as CSS variables
		for (const [key, value] of Object.entries(colorSchemeDev.value)) {
		  document.documentElement.style.setProperty(`--color-${key}`, value)
		}
	  })
}
