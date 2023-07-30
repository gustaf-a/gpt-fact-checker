<script setup lang="ts">
import Container from "../Container.vue";
import useColorScheme from '@/stores/colorScheme'

//Ensure CSS Variables are created
useColorScheme()
</script>

<template>
	<a-layout-header class="hero-banner">
		<Container>
			<div class="hero-container">
				<h1 class="hero-title">Fact Friend</h1>
				<p class="hero-subtext">
					Clearly presented claims - easy to check
				</p>
			</div>
		</Container>
	</a-layout-header>
</template>
<style scoped>
.hero-banner {
	background-color: var(--color-background);
	height: 8vh;
}

h1{
	margin:0;
	padding:0;
}

.hero-container {
	display: flex;
	justify-content: center;
}

.hero-title {
	color: var(--color-text);
	font-size: 1.5em;
	font-weight: bold;
}

.hero-subtext {
	color: var(--color-text);
	margin-left: 2vw;
	font-size: 1.5em;
}
</style>
