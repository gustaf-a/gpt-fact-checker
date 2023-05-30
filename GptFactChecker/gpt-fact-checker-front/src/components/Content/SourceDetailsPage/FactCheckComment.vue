<script setup lang="ts">
import type ClaimCheck from "@/model/ClaimCheck";

interface Props {
	claimCheck: ClaimCheck;
	depth: number;
}

const { claimCheck, depth } = defineProps<Props>();
</script>

<template>
	<a-comment>
		<template #actions>
			<span key="comment-nested-reply-to">Reply to</span>
		</template>
		<template #author>
			<a>User name</a>
		</template>
		<!-- <template #avatar>
			<a-avatar
				src="https://joeschmoe.io/api/v1/random"
				alt="Han Solo"
			/>
		</template> -->
		<template #content>
			<p>
				We supply a series of design principles, practical patterns and high
				quality design resources (Sketch and Axure).
			</p>
		</template>

		<!-- <a-comment>
			<template #actions>
				<span>Reply to</span>
			</template>
			<template #author>
				<a>Han Solo</a>
			</template>
			<template #avatar>
				<a-avatar
					src="https://joeschmoe.io/api/v1/random"
					alt="Han Solo"
				/>
			</template>
			<template #content>
				<p>
					We supply a series of design principles, practical patterns and high
					quality design resources (Sketch and Axure).
				</p>
			</template>
			<a-comment>
				<template #actions>
					<span>Reply to</span>
				</template>
				<template #author>
					<a>Han Solo</a>
				</template>
				<template #avatar>
					<a-avatar
						src="https://joeschmoe.io/api/v1/random"
						alt="Han Solo"
					/>
				</template>
				<template #content>
					<p>
						We supply a series of design principles, practical patterns and high
						quality design resources (Sketch and Axure).
					</p>
				</template>
			</a-comment>
		</a-comment> -->
	</a-comment>
</template>
